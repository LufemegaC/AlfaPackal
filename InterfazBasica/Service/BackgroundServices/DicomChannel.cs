using FellowOakDicom;
using InterfazBasica_DCStore.Models.Dicom;
using InterfazBasica_DCStore.Service.IService;
using InterfazBasica_DCStore.Service.IService.Dicom;
using InterfazBasica_DCStore.Utilities;
using System.Threading.Channels;

namespace InterfazBasica_DCStore.Service.BackgroundServices
{
    /// <summary>
    /// This BackgroundService listens to a channel of DicomFile elements, grouping them into DicomBatch objects based on StudyUID.
    /// Once a batch either reaches certain limits (weight or number of instances), or expires by inactivity (10s),
    /// the batch is sent to a second channel for further processing.
    /// </summary>
    public class DicomChannel : BackgroundService
    {
        // --- CONSTANTS (rules) ---
        private const int MAX_INSTANCES = 8;            // Hard limit on number of instances in a single batch
        private const decimal MIN_WEIGHT_MB = 50m;      // Preferred max weight
        private const decimal MAX_WEIGHT_MB = 70m;      // Absolute max weight (any instance that exceeds this triggers a new batch)
        private const int INACTIVITY_SECONDS = 10;      // Time-based limit for inactivity

        // Log process (DicomChannelSingleton)
        private readonly ILogger<DicomChannel> _logger;

        // DicomFile channel 
        private readonly ChannelReader<DicomFile> _incomingReader;
        // DicomBatch channel (DicomBatchChannelSingleton)
        private readonly ChannelWriter<DicomBatch> _completedWriter;

        // Active batches, keyed by StudyUID
        private readonly Dictionary<string, (DicomBatch batch, CancellationTokenSource timerCts)> _activeBatches
            = new Dictionary<string, (DicomBatch, CancellationTokenSource)>();

        public DicomChannel(ILogger<DicomChannel> logger)
        {
            _logger = logger;
            // reference to the static singletons Channels
            _incomingReader = DicomChannelSingleton.DicomChannel.Reader;
            _completedWriter = DicomBatchChannelSingleton.DicomBatchChannel.Writer;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DicomChannelBackgroundService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Read a new DicomFile from the channel (blocks if empty)
                    var dicomFile = await _incomingReader.ReadAsync(stoppingToken);
                    // Extract the StudyUID (or default)
                    var studyUid = dicomFile.Dataset.GetString(DicomTag.StudyInstanceUID) ?? "NO_STUDY";
                    _logger.LogInformation("studyUid: "+ studyUid);
                    decimal fileSizeMB = GetFileSizeInMB(dicomFile);
                    DicomBatch dicomBatch;
                    /// If the single file size is bigger than MIN_WEIGHT_MB, we handle it separately, as requested.
                    if (fileSizeMB > MIN_WEIGHT_MB)
                    {
                        dicomBatch = new DicomBatch();
                        dicomBatch.Instances.Add(dicomFile);
                        dicomBatch.TotalSizeMB += fileSizeMB;
                        await SendBatchAsync(dicomBatch, stoppingToken);
                        break;
                    }
                    
                    // Check if there's an existing batch for that StudyUID
                    if (!_activeBatches.TryGetValue(studyUid, out var entry))
                        dicomBatch = new DicomBatch();
                    else // get the one
                    {
                        entry.timerCts.Cancel();
                        dicomBatch = entry.batch;
                    }
                    // Create a new timer for inactivity
                    var cts = new CancellationTokenSource();
                    // Add to dictionary
                    _activeBatches[studyUid] = (dicomBatch, cts);
                    // Start monitoring inactivity (fire-and-forget)
                    _ = MonitorInactivityAsync(studyUid, cts.Token);
                    // Now apply the logic of checking if the single instance is bigger than MIN_WEIGHT_MB, etc.
                    await ProcessIncomingFileAsync(dicomBatch, dicomFile, fileSizeMB, studyUid, stoppingToken);
                }
                catch (ChannelClosedException)
                {
                    _logger.LogWarning("DicomChannel is closed. Stopping background service.");
                    break;
                }
                catch (OperationCanceledException)
                {
                    // Service is shutting down
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error while reading from DicomFile channel.");
                }
            }

            // Flush all active batches upon shutdown
            await FlushAllBatchesAsync(stoppingToken);
            _logger.LogInformation("DicomChannelBackgroundService finished.");
        }

        /// <summary>
        /// Process the newly arrived file according to size constraints and instance count constraints.
        /// If adding the file to the existing batch would exceed 70 MB, send the current batch, then start a new one.
        /// If the batch hits the max instance count, send it.
        /// </summary>
        private async Task ProcessIncomingFileAsync(DicomBatch batch, DicomFile dicomFile, decimal fileSizeMB, string studyUid, CancellationToken stoppingToken)
        {

            // 2) If adding this file would exceed 70 MB (MAX_WEIGHT_MB), we send the existing batch first
            decimal projectedSize = batch.TotalSizeMB + fileSizeMB;
            if (projectedSize > MAX_WEIGHT_MB)
            {
                // Send current batch
                await SendBatchAsync(batch, stoppingToken);
                _activeBatches.Remove(studyUid);
                //var transactionUID = GenerateNewTransactionUID();
                // Create a new batch for this file
                var newBatch = new DicomBatch();
                newBatch.Instances.Add(dicomFile);
                newBatch.TotalSizeMB += fileSizeMB;
                var cts = new CancellationTokenSource();
                _activeBatches[studyUid] = (newBatch, cts);
                _ = MonitorInactivityAsync(studyUid, cts.Token);
                return;
            }

            // 3) Otherwise, we can safely add the file to the batch
            batch.Instances.Add(dicomFile);
            batch.TotalSizeMB += fileSizeMB;

            // 4) Check if we reached max instance count
            if (batch.Instances.Count >= MAX_INSTANCES)
            {
                await SendBatchAsync(batch, stoppingToken);
                _activeBatches.Remove(studyUid);
            }
        }
        /// <summary>
        /// Monitors inactivity. If no new file arrives for 'INACTIVITY_SECONDS' for this StudyUID,
        /// we send the batch. The underscore '_' discards the Task's result (fire-and-forget).
        /// </summary>
        private async Task MonitorInactivityAsync(string studyUid, CancellationToken token)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(INACTIVITY_SECONDS), token);
                // If we reach here, the batch is inactive

                if (_activeBatches.TryGetValue(studyUid, out var entry))
                {
                    _logger.LogInformation("Inactivity reached for StudyUID={0}, sending batch...", studyUid);
                    await SendBatchAsync(entry.batch, CancellationToken.None);
                    _activeBatches.Remove(studyUid);
                }
            }
            catch (TaskCanceledException)
            {
                // Timer was reset by a new file arrival
            }
        }

        /// <summary>
        /// Writes the batch to the 'DicomBatchChannelSingleton.DicomBatchChannel' for further processing.
        /// </summary>
        private async Task SendBatchAsync(DicomBatch batch, CancellationToken stoppingToken)
        {
            if (batch.Instances.Count == 0)
            {
                _logger.LogWarning("Attempting to send an empty batch. Skipping.");
                return;
            }

            _logger.LogInformation(
                "Sending batch [StudyUID={0}] with {1} instances, total size={2} MB.",
                batch.StudyUID,
                batch.Instances.Count,
                batch.TotalSizeMB);

            try
            {
                await _completedWriter.WriteAsync(batch, stoppingToken);
            }
            catch (ChannelClosedException)
            {
                _logger.LogWarning("DicomBatch channel is closed. Cannot send batch.");
            }
        }

        /// <summary>
        /// When the service stops, we flush all active batches.
        /// </summary>
        private async Task FlushAllBatchesAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Flushing all active batches before stopping...");

            foreach (var kvp in _activeBatches)
            {
                var studyUid = kvp.Key;
                var (batch, _) = kvp.Value;

                if (batch.Instances.Count > 0)
                {
                    await SendBatchAsync(batch, stoppingToken);
                }
            }

            _activeBatches.Clear();
        }

        private decimal GetFileSizeInMB(DicomFile dicomFile)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    // Guarda el archivo en el MemoryStream
                    dicomFile.Save(memoryStream);
                    // El tamaño del archivo DICOM en bytes
                    long fileSizeInBytes = memoryStream.Length;
                    decimal fileSizeInMB = (decimal)fileSizeInBytes / (1024 * 1024);
                    return fileSizeInMB;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or throw it, depending on your logging strategy
                Console.WriteLine($"Error al obtener el tamaño del archivo: {ex.Message}");
                // Puedes lanzar la excepción nuevamente o manejarla según sea necesario
                throw;
            }
        }
    }
}
