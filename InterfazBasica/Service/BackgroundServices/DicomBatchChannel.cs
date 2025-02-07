using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Models.Dicom;
using InterfazBasica_DCStore.Models.Dicom.Web;
using InterfazBasica_DCStore.Service.IService;
using InterfazBasica_DCStore.Service.IService.Dicom;
using InterfazBasica_DCStore.Utilities;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Channels;

namespace InterfazBasica_DCStore.Service.BackgroundServices
{
    public class DicomBatchChannel : BackgroundService
    {
        private readonly ILogger<DicomBatchChannel> _logger;
        private readonly IDicomDecompositionService _decompositionService;
        private readonly IDicomWebService _dicomWebService;
        private readonly ILocalDicomStorageService _localDicomStorageService;

        private string _storagePath;
        private string _failedInstances;

        // DicomFile channel 
        private readonly ChannelReader<DicomBatch> _incomingReader;

        public DicomBatchChannel(ILogger<DicomBatchChannel> logger,
            IConfiguration configuration,
            IDicomDecompositionService decompositionService,
            IDicomWebService dicomWebService,
            ILocalDicomStorageService localDicomStorageService)
        {
            _logger = logger;
            _storagePath = configuration.GetValue<string>("DicomSettings:StoragePath");
            _failedInstances = configuration.GetValue<string>("DicomSettings:FailedInstances");
            _incomingReader = DicomBatchChannelSingleton.DicomBatchChannel.Reader;
            _decompositionService = decompositionService;
            _dicomWebService = dicomWebService;
            _localDicomStorageService = localDicomStorageService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DicomChannelBackgroundService started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                // Read a new DicomFile from the channel (blocks if empty)
                var dicomBatch = await _incomingReader.ReadAsync(stoppingToken);
                _logger.LogInformation("Batch size: " + dicomBatch.Instances.Count.ToString());
                var stowRsResponde = await SendInstancesToDicomWeb(dicomBatch);
                await StoreInstancesOnLocalDisk(stowRsResponde, dicomBatch.Instances);
            }
        }

        /// <summary>
        /// Stores DICOM instances locally based on their acceptance status from a STOW-RS response.
        /// Accepted instances are stored in the "Accepted" folder, while failed instances are stored in the "Rejected" folder.
        /// </summary>
        /// <param name="stowRsResponse">The STOW-RS response containing lists of accepted and failed instances.</param>
        /// <param name="dicomFiles">The list of DICOM files from the DICOM batch.</param>
        /// <returns>A task representing the asynchronous operation of storing DICOM files.</returns>
        internal async Task<DicomStatus> StoreInstancesOnLocalDisk(StowRsResponse stowRsResponse, List<DicomFile> dicomFiles)
        {
            if (stowRsResponse == null)
                throw new ArgumentNullException(nameof(stowRsResponse));

            if (dicomFiles == null || !dicomFiles.Any())
                throw new ArgumentException("The list of DICOM files is null or empty.");


            // Store accepted instances
            foreach (var acceptedInstance in stowRsResponse.AcceptedSOPSequence)
            {
                var dicomFile = dicomFiles.FirstOrDefault(file =>
                    file.Dataset.GetString(DicomTag.SOPInstanceUID) == acceptedInstance.SOPInstanceUID);

                if (dicomFile != null)
                {
                    await _localDicomStorageService.StoreDicomFileAsync(dicomFile, _storagePath);
                }
            }

            // Store failed instances
            foreach (var failedInstance in stowRsResponse.FailedSOPSequence)
            {
                var dicomFile = dicomFiles.FirstOrDefault(file =>
                    file.Dataset.GetString(DicomTag.SOPInstanceUID) == failedInstance.SOPInstanceUID);

                if (dicomFile != null)
                {
                    await _localDicomStorageService.StoreDicomFileAsync(dicomFile, _failedInstances);
                }
            }

            return DicomStatus.Success;
        }



        /// <summary>
        /// Sends a list of DICOM instances to a DICOMweb server using STOW-RS.
        /// </summary>
        /// <param name="instances">List of DICOM files to be sent.</param>
        /// <returns>A <see cref="StowRsResponse"/> indicating the result of the operation.</returns>
        internal async Task<StowRsResponse> SendInstancesToDicomWeb(DicomBatch dicomBatch)
        {
            try
            {
                // Generate a unique boundary for the multipart content
                // generate Transaction info
                //string transactionUID = DicomUID.Generate().UID;
                var boundary = $"PACKAL_{DateTime.UtcNow:yyyyMMddHHmmss}";
                var content = new MultipartContent("related", boundary);
                content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("type", "\"application/dicom\""));
                // prepare instances
                foreach (var instance in dicomBatch.Instances)
                {
                    // Extract the DICOM dataset
                    var dicomDataset = instance.Dataset;
                    // convert dicomDataSet to Dicom+json ( Not all metadatas )
                    var metadataJson = _decompositionService.ConvertDatasetToJson(dicomDataset);
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
    }
}
