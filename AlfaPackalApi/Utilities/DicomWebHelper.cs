using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.DicomWeb;
using FellowOakDicom;
using Newtonsoft.Json;

namespace Api_PACsServer.Utilities
{
    public class DicomWebHelper
    {
        /// <summary>
        /// Parses a STOW-RS request and extracts DICOM files and associated metadata.
        /// </summary>
        /// <param name="request">The HTTP request containing the multipart/related content.</param>
        /// <returns>A list of StowRsRequestDto instances representing the DICOM files and metadata.</returns>
        public static async Task<List<StowRsRequestDto>> ParseStowRsRequest(HttpRequest request)
        {
            var result = new List<StowRsRequestDto>();

            if (!request.HasFormContentType)
                throw new InvalidOperationException("Request does not contain multipart/form-data content.");

            var form = await request.ReadFormAsync();

            // List to hold metadata JSON if provided
            List<MetadataDto> metadataList = null;

            // First, process any metadata parts
            foreach (var file in form.Files)
            {
                if (file.ContentType == "application/dicom+json")
                {
                    // Read the metadata JSON
                    using var reader = new StreamReader(file.OpenReadStream());
                    var json = await reader.ReadToEndAsync();

                    // Deserialize the JSON into a list of MetadataDto
                    metadataList = JsonConvert.DeserializeObject<List<MetadataDto>>(json);
                }
            }

            int index = 0;

            // Then, process DICOM files
            foreach (var file in form.Files)
            {
                if (file.ContentType == "application/dicom")
                {
                    var stowRsRequest = new StowRsRequestDto
                    {
                        DicomFile = file
                    };

                    // Associate metadata if available
                    if (metadataList != null && index < metadataList.Count)
                    {
                        stowRsRequest.Metadata = metadataList[index];
                    }

                    result.Add(stowRsRequest);
                    index++;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a list of OperationResult into a DICOMWeb STOW-RS response JSON.
        /// </summary>
        /// <param name="operationResults">List of OperationResult instances.</param>
        /// <returns>A JSON string representing the STOW-RS response.</returns>
        public static string CreateStowRsResponse(List<DicomOperationResult> operationResults)
        {
            var acceptedInstances = operationResults
                .Where(r => r.IsSuccess)
                .Select(r => new AcceptedInstance(r))
                .ToList();

            var failedInstances = operationResults
                .Where(r => !r.IsSuccess)
                .Select(r => new FailedInstance(r))
                .ToList();

            var response = new StowRsResponse(acceptedInstances, failedInstances);
            // Serialize the response to JSON
            return JsonConvert.SerializeObject(response);
        }


        private static readonly HashSet<string> SupportedSopClasses = new HashSet<string>
        {
            DicomUID.CTImageStorage.UID,                         // Tomografía Computarizada
            DicomUID.MRImageStorage.UID,                         // Resonancia Magnética
            DicomUID.RTImageStorage.UID, 
            DicomUID.UltrasoundImageStorage.UID,                 // Ultrasonido
            DicomUID.SecondaryCaptureImageStorage.UID,           // Captura Secundaria
            DicomUID.DigitalXRayImageStorageForPresentation.UID, // Radiografía Digital para Presentación
            DicomUID.DigitalMammographyXRayImageStorageForPresentation.UID, // Mamografía Digital para Presentación
            DicomUID.NuclearMedicineImageStorage.UID,            // Medicina Nuclear
            DicomUID.EnhancedCTImageStorage.UID,                 // Imágenes CT Mejoradas
            DicomUID.EnhancedMRImageStorage.UID,                 // Imágenes MR Mejoradas
            DicomUID.EnhancedUSVolumeStorage.UID                // Volumen de Ultrasonido Mejorado
        };

        private static readonly HashSet<string> SupportedTransferSyntaxes = new HashSet<string>
        {
            DicomUID.ExplicitVRLittleEndian.UID,
            //DicomUID.ExplicitVRBigEndian.UID, //RETIRED
            DicomUID.ImplicitVRLittleEndian.UID,
            // Add other supported Transfer Syntaxes
        };

        public static class DicomErrorCodes
        // Last update with DICOM PS3.18 2024a - Web Services
        // Table I.2-2. Store Instances Response Failure Reason Values
        {
            public const int ProcessingFailure = 272;             // Error de procesamiento (0x0110)
            public const int CannotUnderstand = 49152;            // No se puede entender (0xC000)
            public const int OutOfResources = 42752;              // Sin recursos (0xA700)
            public const int DataSetDoesNotMatchSOPClass = 43264; // El dataset no coincide con el SOP Class (Archivo corrupto) (0xA900)
            public const int TransferSyntaxNotSupported = 49442;  // Transfer Syntax no soportado (0x0122)
            public const int SOPClassUIDNotSupported = 290;       // SOP Class UID no soportado 
        }
    }
}
