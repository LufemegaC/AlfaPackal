using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Network;
using FellowOakDicom.Serialization;
using InterfazBasica_DCStore.Models.Dicom.Web;
using InterfazBasica_DCStore.Service.IService;
using InterfazBasica_DCStore.Service.IService.Dicom;
using InterfazBasica_DCStore.Utilities;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using static InterfazBasica_DCStore.Utilities.LocalUtility;

namespace InterfazBasica_DCStore.Service
{
    public class DicomOrchestrator : IDicomOrchestrator
    {
        private readonly IDicomValidationService _validationService;
        private readonly IDicomDecompositionService _decompositionService;
        private readonly IDicomWebService _dicomWebService;
        private string _rootPath;
        private string _requestPath;
        private string _failedInstances;
        private string _transactionUID;
        //private int _institutionID;
        private readonly ILogger<DicomOrchestrator> _logger;

        // Values for sends control
        private List<DicomFile> _accumulatedDicomFiles = new List<DicomFile>(); // Dicom files list
        private decimal _totalAccumulatedSize = 0; // Total size of acccumulated instances
        private Timer _timer; // Timer
        private readonly decimal _maxAccumulatedSize = 20; // Max size for request (MB)
        private readonly double _timeout = 10000; // 10 seconds
        private int _instanceCounter = 0;

        public DicomOrchestrator(
            IDicomValidationService validationService,
            IDicomDecompositionService decompositionService,
            IDicomWebService dicomWebService,
            IConfiguration configuration,
            ILogger<DicomOrchestrator> logger)
        {
            _validationService = validationService;
            _decompositionService = decompositionService;
            _dicomWebService = dicomWebService;
            _rootPath = configuration.GetValue<string>("DicomSettings:StoragePath");
            _requestPath = configuration.GetValue<string>("DicomSettings:StowRsPath");
            _failedInstances = configuration.GetValue<string>("DicomSettings:FailedInstances");
            //services.Configure<DicomSettings>(Configuration.GetSection("DicomSettings"));

            // Inicializamos el temporizador pero sin arrancarlo aún
            _timer = new Timer(OnTimeoutElapsed, null, Timeout.Infinite, Timeout.Infinite);
            _logger = logger;
        }

        public string GetServerAEtitle()
        {
            return Aetitle;
        }

        public async Task<DicomStatus> StoreDicomData(DicomFile dicomFile)
        {
            try
            {
                // Generar TransactionUID si aún no se ha generado
                if (string.IsNullOrEmpty(_transactionUID))
                {
                    _transactionUID = GenerateNewTransactionUID();
                }
                // add TransactionUID to DicomFile
                dicomFile.Dataset.AddOrUpdate(DicomTag.TransactionUID, _transactionUID);
                dicomFile.Dataset.AddOrUpdate(DicomTag.TransactionStatus, "IN_PROGRESS_SCP");
                var sopInstanceUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, null);
                _logger.LogInformation("Starting storage process for SOPInstanceUID: {SOPInstanceUID}", sopInstanceUID);
                // Detenemos el temporizador inmediatamente
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                // Validación inicial del archivo DICOM.
                var dicomStatus = _validationService.IsValidDicomFile(dicomFile);
                if (dicomStatus != DicomStatus.Success)
                {
                    dicomFile.Dataset.AddOrUpdate(DicomTag.TransactionStatus, "NOT_VALID");
                    await StoreDicomOnlocalDisk(dicomFile, StorageLocation.Failed); // Store invalid file in failed instances directory
                    return dicomStatus;
                }       
                dicomFile.Dataset.AddOrUpdate(DicomTag.TransactionStatus, "VALIDATED");
                dicomStatus = await StoreDicomOnlocalDisk(dicomFile, StorageLocation.Temporary);
                decimal fileSizeMb = _decompositionService.GetFileSizeInMB(dicomFile);
                dicomFile.Dataset.AddOrUpdate(DicomTag.TransactionStatus, "PREPARED_SCP");
                _totalAccumulatedSize += fileSizeMb;
                if (_totalAccumulatedSize > _maxAccumulatedSize)
                {
                    _logger.LogInformation("Accumulated size exceeds threshold. Sending instances to DICOMweb.");
                    var dicomFiles = LoadDicomFilesFromTemporaryDirectory();
                    var resultDicomWeb = await SendInstancesToDicomWeb(dicomFiles);
                    // Handle accepted and failed instances from DICOMweb response
                    await HandleStowRsResponse(resultDicomWeb, dicomFiles);
                    _transactionUID = "";
                    // Clean up temporary files after processing
                    DeleteTemporaryFiles();
                }
                else
                {
                    // Restart the timer for 10 seconds if the size threshold has not been met
                    _logger.LogInformation("Accumulated size exceeds threshold. Sending instances to DICOMweb.");
                    _timer.Change((int)_timeout, Timeout.Infinite);
                }
                _logger.LogInformation("End of method {Timeout} ms.", _timeout);
                return DicomStatus.Success;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el almacenamiento de la instancia DICOM.");
                return DicomStatus.Cancel;
            }
        }

        /// <summary>
        /// Handles the event when the timer elapses, sending accumulated DICOM instances to the DICOMweb server.
        /// </summary>
        /// <param name="state">The state object associated with the timer.</param>
        public void OnTimeoutElapsed(object state)
        {
            try
            {
                _logger.LogInformation("Timeout elapsed. Sending accumulated instances to DICOMweb.");
                var dicomFiles = LoadDicomFilesFromTemporaryDirectory();
                if (dicomFiles.Any())
                {
                    var resultDicomWeb = SendInstancesToDicomWeb(dicomFiles).GetAwaiter().GetResult();

                    // Handle accepted and failed instances from DICOMweb response
                    HandleStowRsResponse(resultDicomWeb, dicomFiles).GetAwaiter().GetResult();
                    _transactionUID = "";
                    // Clean up temporary files after processing
                    DeleteTemporaryFiles();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during OnTimeoutElapsed.");
            }
        }

        /// <summary>
        /// Sends a list of DICOM instances to a DICOMweb server using STOW-RS.
        /// </summary>
        /// <param name="instances">List of DICOM files to be sent.</param>
        /// <returns>A <see cref="StowRsResponse"/> indicating the result of the operation.</returns>
        internal async Task<StowRsResponse> SendInstancesToDicomWeb(List<DicomFile> instances)
        {
            try
            {
                // Generate a unique boundary for the multipart content
                // generate Transaction info
                string transactionUID = DicomUID.Generate().UID;
                var boundary = $"PACKAL_{DateTime.UtcNow:yyyyMMddHHmmss}";
                var content = new MultipartContent("related", boundary);
                content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("type", "\"application/dicom\""));
                // prepare instances
                foreach (var instance in instances)
                {
                    // Extract the DICOM dataset
                    var dicomDataset = instance.Dataset;
                    // Extract metadata from the DICOM file
                    var metadata = _decompositionService.ExtractMetadata(dicomDataset);
                    // Convert the metadata dictionary to a DTO model
                    var metadataDto = _decompositionService.DicomDictionaryToCreateEntities(metadata);
                    // Convert DTO to DICOM JSON using the DicomJsonConverter
                    var metadataJsonObject = DicomUtility.ConvertDtoToDicomJson(metadataDto);
                    var metadataJson = metadataJsonObject.ToString();
                    // Create content for the DICOM file
                    using (var memoryStream = new MemoryStream())
                    {
                        instance.Save(memoryStream);
                        var dicomContent = new ByteArrayContent(memoryStream.ToArray());
                        dicomContent.Headers.ContentType = new MediaTypeHeaderValue("application/dicom");
                        content.Add(dicomContent);
                    }
                    // Create content for the metadata JSON
                    var jsonContent = new StringContent(metadataJson, Encoding.UTF8, "application/dicom+json");
                    content.Add(jsonContent);
                }
                // Send the request to the DICOMweb server
                return await _dicomWebService.RegisterInstances(content);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        internal async Task<DicomStatus> StoreDicomOnlocalDisk(DicomFile dicomFile, StorageLocation location )
        {
            string instanceUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, null);
            string studyUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.StudyInstanceUID, null);
            string seriesUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SeriesInstanceUID, null);
            string tempFilePath = PathFileDicomConstructor(studyUID, seriesUID, instanceUID, location);
            // Guarda el archivo DICOM en la ruta especificada.
            await dicomFile.SaveAsync(tempFilePath);
            // Retorna una respuesta de éxito.
            return DicomStatus.Success;
        }

        /// <summary>
        /// Generates a random alphanumeric boundary string.
        /// </summary>
        /// <returns>A random boundary string.</returns>
        private static string GenerateRandomBoundary()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 16).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal string PathFileDicomConstructor(string StudyUID, string SerieUID, string InstanceUID, StorageLocation location)
        {
            // Seleccionar la ruta base según la ubicación (temporal o permanente)
            string fullPath;
            if (location == StorageLocation.Permanent)
            {
                fullPath = Path.Combine(_rootPath, StudyUID, SerieUID);
            }
            else if(location == StorageLocation.Failed)
            {
                fullPath = _failedInstances;
            }
            else
            {
                fullPath = _requestPath;
            }
            // Verifica si la ruta del directorio existe, si no, la crea.
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            // Completa la ruta del archivo añadiendo el SOPInstanceUID y la extensión .dcm.
            var filePath = Path.Combine(fullPath, InstanceUID + ".dcm");
            return filePath;
        }

        /// <summary>
        /// Loads all DICOM files from the temporary directory.
        /// </summary>
        /// <returns>A list of <see cref="DicomFile"/> loaded from the temporary directory.</returns>
        internal List<DicomFile> LoadDicomFilesFromTemporaryDirectory()
        {
            string tempPath = _requestPath; // 
            List<DicomFile> dicomFiles = new List<DicomFile>();

            string[] files = Directory.GetFiles(tempPath);
            foreach (string file in files)
            {
                if (Path.GetExtension(file).Equals(".dcm", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var dicomFile = DicomFile.Open(file);
                        dicomFiles.Add(dicomFile);
                    }
                    catch (Exception ex)
                    {
                        // Log the error or handle it appropriately
                        Console.WriteLine($"Error loading DICOM file {file}: {ex.Message}");
                    }
                }
            }
            return dicomFiles;
        }

        private async Task HandleStowRsResponse(StowRsResponse response, List<DicomFile> dicomFiles)
        {
            if (response.AcceptedSOPSequence != null && response.AcceptedSOPSequence.Count >0)
            {
                foreach (var acceptedInstance in response.AcceptedSOPSequence)
                {
                    var acceptedDicomFile = dicomFiles.FirstOrDefault(file =>
                        file.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, null) == acceptedInstance.SOPInstanceUID);
                    if (acceptedDicomFile != null)
                    {
                        acceptedDicomFile.Dataset.AddOrUpdate(DicomTag.TransactionStatus, "STORAGED");
                        await StoreDicomOnlocalDisk(acceptedDicomFile, StorageLocation.Permanent);
                    }
                }
            }
            if (response.FailedSOPSequence != null && response.FailedSOPSequence.Count >0)
            { 
                foreach (var failedInstance in response.FailedSOPSequence)
                {
                    var failedDicomFile = dicomFiles.FirstOrDefault(file =>
                        file.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, null) == failedInstance.SOPInstanceUID);
                    if (failedDicomFile != null)
                    {
                        failedDicomFile.Dataset.AddOrUpdate(DicomTag.TransactionStatus, "STOREWEB_FAILED");
                        failedDicomFile.Dataset.AddOrUpdate(DicomTag.TransactionStatusComment, failedInstance.ErrorMessage);
                        await StoreDicomOnlocalDisk(failedDicomFile, StorageLocation.Failed);
                    }
                }
            }
            // Clean up temporary files after processing
            DeleteTemporaryFiles();

        }

        private void DeleteTemporaryFiles()
        {
            string[] files = Directory.GetFiles(_requestPath, "*.dcm");
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting temporary file: {File}", file);
                }
            }
            _totalAccumulatedSize = 0; //Clear value
        }


        private string GenerateNewTransactionUID()
        {
            return DicomUID.Generate().UID;
        }
    }
}
