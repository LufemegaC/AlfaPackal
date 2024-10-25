using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Models.Dto.Studies;
using FellowOakDicom;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Api_PACsServer.Utilities
{
    public class DicomWebHelper
    {
        /// *** STOW-RS SECCION *** ///

        /// <summary>
        /// Parses a STOW-RS request and extracts DICOM files and associated metadata.
        /// </summary>
        /// <param name="request">The HTTP request containing the multipart/related content.</param>
        /// <returns>A list of StowRsRequestDto instances representing the DICOM files and metadata.</returns>
        public static async Task<StowRsRequestDto> ParseStowRsRequest(HttpRequest request)
        {
            var result = new StowRsRequestDto();

            // Check if the request content type is multipart/related
            if (!request.ContentType.Contains("multipart/related", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Request does not contain multipart/related content.");

            // Extract the boundary from the Content-Type header
            var boundary = GetBoundaryFromContentType(request.ContentType);
            if (string.IsNullOrEmpty(boundary))
                throw new InvalidOperationException("Boundary not found in Content-Type header.");


            // Create a MultipartReader to read each section of the multipart content
            var reader = new MultipartReader(boundary, request.Body);
            MultipartSection section;

            var metadataList = new List<MetadataDto>();
            var instancesList = new List<DicomFile>();
            long fileSize = 0; // Initialize fileSize to avoid unassigned variable error
            string transactionUID = null;

            // Iterate through each section of the multipart content
            while ((section = await reader.ReadNextSectionAsync()) != null)
            {
                var contentType = section.ContentType;
                if (contentType == "application/dicom")
                {
                    using var memoryStream = new MemoryStream();
                    await section.Body.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    fileSize = memoryStream.Length;
                    var dicomFile = DicomFile.Open(memoryStream);
                    instancesList.Add(dicomFile);
                    // Extract TransactionUID from the DICOM file if not already set
                    if (transactionUID == null)
                    {
                        transactionUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.TransactionUID, null);
                    }
                }

                else if(contentType.StartsWith("application/dicom+json", StringComparison.OrdinalIgnoreCase))
                {
                    // Read the metadata JSON
                    using var readerStream = new StreamReader(section.Body);
                    var json = await readerStream.ReadToEndAsync();

                    // Parse JSON into JObject and convert to MetadataDto
                    var dicomJson = JObject.Parse(json);
                    var metadataDto = ConvertDicomJsonToDto(dicomJson);
                    metadataDto.TotalFileSizeMB = Math.Round((decimal)fileSize / (1024 * 1024), 2);
                    metadataList.Add(metadataDto);
                }

            }
            result.DicomFilesPackage = CombineMetadataAndDicomFiles(metadataList, instancesList);
            result.TransactionUID = transactionUID;
            return result;
        }

        /// <summary>
        /// Combines metadata and DICOM files into a list of DicomFilePackage.
        /// </summary>
        /// <param name="metadataList">List of MetadataDto containing the metadata information.</param>
        /// <param name="dicomFiles">List of IFormFile representing the DICOM files.</param>
        /// <returns>A list of DicomFilePackage combining the metadata and DICOM files.</returns>
        internal static List<DicomFilePackage> CombineMetadataAndDicomFiles(List<MetadataDto> metadataList, List<DicomFile> dicomFiles)
        {
            var result = new List<DicomFilePackage>();

            if (metadataList.Count != dicomFiles.Count)
            {
                throw new InvalidOperationException("The number of metadata entries does not match the number of DICOM files.");
            }

            for (int i = 0; i < metadataList.Count; i++)
            {
                var stowRequest = new DicomFilePackage
                {
                    Metadata = metadataList[i],
                    DicomFile = dicomFiles[i],
                    //TotalFileSizeMB = Math.Round((decimal)instancesList[i].fileSize / (1024 * 1024), 2) // Convert bytes to MB
                };
                result.Add(stowRequest);
            };

            return result;
        }


        /// <summary>
        /// Converts a list of OperationResult into a DICOMWeb STOW-RS response JSON.
        /// </summary>
        /// <param name="operationResults">List of OperationResult instances.</param>
        /// <returns>A JSON string representing the STOW-RS response.</returns>
        public static string CreateStowRsResponse(List<DicomOperationResult> operationResults, string transactionUID)
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
            response.TransactionUID = transactionUID;
            // STATUS CONFIGURATION
            if (failedInstances.Count == 0 && acceptedInstances.Count > 0)
            {
                response.Status = "200 (OK)"; // All instances successfully stored
            }
            else if (failedInstances.Count > 0 && acceptedInstances.Count > 0)
            {
                response.Status = "202 (Accepted)"; // Some instances stored, some failed
            }
            else if (failedInstances.Count > 0 && acceptedInstances.Count == 0)
            {
                response.Status = "409 (Conflict)"; // All instances failed due to a conflict
            }
            else
            {
                response.Status = "400 (Bad Request)"; // Bad request, unable to store any instances
            }
            // Serialize the response to JSON
            return JsonConvert.SerializeObject(response);
        }

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

        /// <summary>
        /// Extracts the boundary from the Content-Type header of the request.
        /// </summary>
        /// <param name="contentType">The Content-Type header value.</param>
        /// <returns>The boundary string if found; otherwise, null.</returns>
        private static string GetBoundaryFromContentType(string contentType)
        {
            var elements = contentType.Split(';');
            foreach (var element in elements)
            {
                var trimmedElement = element.Trim();
                if (trimmedElement.StartsWith("boundary="))
                {
                    return trimmedElement.Substring("boundary=".Length).Trim('"');
                }
            }
            return null;
        }



        /// *** STOW-RS SECCION : ENDS *** ///

        /// *** QIDO-RS SECCION : BEGINS *** ///

        /// <summary>
        /// Maps the query parameters to ControlQueryParametersDto and StudyQueryParametersDto.
        /// </summary>
        /// <param name="queryParams">Dictionary containing query parameters from the request.</param>
        /// <returns>Tuple containing ControlQueryParametersDto and StudyQueryParametersDto.</returns>
        public static (ControlQueryParametersDto, StudyQueryParametersDto) MapQueryParamsToDtos(Dictionary<string, string> queryParams)
        {
            var controlParamsDto = new ControlQueryParametersDto();
            var studyParamsDto = new StudyQueryParametersDto();

            foreach (var param in queryParams)
            {
                // Si el parámetro es un parámetro de control, asignarlo al ControlQueryParametersDto
                if (controlParameters.Contains(param.Key.ToLower()))
                {
                    switch (param.Key.ToLower())
                    {
                        case "limit":
                            controlParamsDto.Limit = param.Value;
                            break;
                        case "order":
                            controlParamsDto.Order = param.Value;
                            break;
                        case "offset":
                            controlParamsDto.Offset = param.Value;
                            break;
                        case "includefields":
                            controlParamsDto.IncludeFields.AddRange(param.Value.Split(',').Select(field => field.Trim()));
                            break;
                        case "format":
                            controlParamsDto.Format = param.Value;
                            break;
                        case "page":
                            controlParamsDto.Page = param.Value;
                            break;
                        case "pagesize":
                            controlParamsDto.PageSize = param.Value;
                            break;
                    }
                }
                // Si el parámetro es un atributo DICOM, asignarlo al StudyQueryParametersDto
                else if (DicomTagToDto.ContainsKey(param.Key) || DicomTagToDto.ContainsValue(param.Key))
                {
                    var propertyKey = DicomTagToDto.ContainsKey(param.Key) ? param.Key : DicomTagToDto.FirstOrDefault(x => x.Value == param.Key).Key;
                    var propertyInfo = studyParamsDto.GetType().GetProperty(propertyKey);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(studyParamsDto, param.Value);
                    }
                }
            }
            return (controlParamsDto, studyParamsDto);
        }

        // List of known control parameters (not part of DICOM tags)
        internal static List<string>  controlParameters = new List<string> 
        { 
            "limit", 
            "orderby", 
            "offset", 
            "includeFields", 
            "format", 
            "page",
            "pageSize" 
        
        };

        internal static Dictionary<string, string> DicomTagToDto = new Dictionary<string, string>
        {
            // Study-level attributes
            { "PatientID", "00100020" },
            { "StudyDate", "00080020" },
            { "AccessionNumber", "00080050" },
            { "StudyInstanceUID", "0020000D" },
            { "PatientName", "00100010" },
            { "PatientAge", "00101010" },
            { "PatientSex", "00100040" },
            { "InstitutionName", "00080080" },
            { "BodyPartExamined", "00180015" },

            // Series-level attributes
            { "SeriesInstanceUID", "0020000E" },
            { "SeriesNumber", "00200011" },
            { "Modality", "00080060" },
            { "SeriesDateTime", "00080031" },
            { "PatientPosition", "00185100" },

            // Instance-level attributes
            { "SOPInstanceUID", "00080018" },
            { "SOPClassUID", "00080016" },
            { "TransferSyntaxUID", "00020010" },
            { "InstanceNumber", "00200013" },
            { "ImageComments", "00204000" },
            { "PhotometricInterpretation", "00280004" },
            { "PixelSpacing", "00280030" },
            { "NumberOfFrames", "00280008" },
            { "ImagePositionPatient", "00200032" },
            { "ImageOrientationPatient", "00200037" }
        };

        /// <summary>
        /// Converts a list of StudyDto objects to a DICOM JSON string for QIDO-RS response.
        /// Only essential study attributes are included: PatientName, PatientAge, PatientSex, StudyDate, Modality, BodyPartExamined.
        /// </summary>
        /// <param name="studyDtos">List of StudyDto containing the study metadata.</param>
        /// <returns>A JSON string representing the combined DICOM JSON structure.</returns>
        public static string ConvertStudiesToDicomJsonString(List<StudyDto> studyDtos)
        {
            var dicomJsonArray = new JArray();

            foreach (var study in studyDtos)
            {
                var dicomJson = new JObject();

                // Patient's Name
                if (!string.IsNullOrEmpty(study.PatientName))
                {
                    dicomJson["00100010"] = new JObject
                    {
                        ["vr"] = "PN",
                        ["Value"] = new JArray(study.PatientName)
                    };
                }

                // Patient's Age
                if (!string.IsNullOrEmpty(study.PatientAge))
                {
                    dicomJson["00101010"] = new JObject
                    {
                        ["vr"] = "AS",
                        ["Value"] = new JArray(study.PatientAge)
                    };
                }

                // Patient's Sex
                if (!string.IsNullOrEmpty(study.PatientSex))
                {
                    dicomJson["00100040"] = new JObject
                    {
                        ["vr"] = "CS",
                        ["Value"] = new JArray(study.PatientSex)
                    };
                }

                // Study Date
                if (study.StudyDate != DateTime.MinValue)
                {
                    dicomJson["00080020"] = new JObject
                    {
                        ["vr"] = "DA",
                        ["Value"] = new JArray(study.StudyDate.ToString("yyyyMMdd"))
                    };
                }

                // Modality
                if (!string.IsNullOrEmpty(study.Modality))
                {
                    dicomJson["00080060"] = new JObject
                    {
                        ["vr"] = "CS",
                        ["Value"] = new JArray(study.Modality)
                    };
                }

                // Body Part Examined
                if (!string.IsNullOrEmpty(study.BodyPartExamined))
                {
                    dicomJson["00180015"] = new JObject
                    {
                        ["vr"] = "CS",
                        ["Value"] = new JArray(study.BodyPartExamined)
                    };
                }

                dicomJsonArray.Add(dicomJson);
            }

            var result = new JObject
            {
                ["Studies"] = dicomJsonArray
            };

            // Convert the JObject to a JSON string
            return JsonConvert.SerializeObject(result);
        }
        /// *** QIDO-RS SECCION : ENDS *** ///

        // --- CONVERTER ZONE --- //
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


        



        // PENDIENTES DEFINIR SU POSICION :

        /// <summary>
        /// Converts a DICOM JSON structure into a MainEntitiesCreateDto object.
        /// </summary>
        /// <param name="dicomJson">The JObject representing the DICOM JSON structure.</param>
        /// <returns>A MetadataDto object containing the metadata for the DICOM instance.</returns>
        public static MetadataDto ConvertDicomJsonToDto(JObject dicomJson)
        {
            var dto = new MetadataDto();

            // SOP Class UID
            if (dicomJson.TryGetValue("00020010", out var sopClassUidToken))
            {
                dto.SOPClassUID = sopClassUidToken["Value"]?.First?.ToString();
            }

            // SOP Instance UID and Series Instance UID
            if (dicomJson.TryGetValue("00081199", out var referencedSeriesToken))
            {
                var referencedSeries = referencedSeriesToken["Value"]?.FirstOrDefault() as JObject;
                if (referencedSeries != null)
                {
                    dto.SOPInstanceUID = referencedSeries["00080016"]?["Value"]?.First?.ToString();
                    dto.SeriesInstanceUID = referencedSeries["00080018"]?["Value"]?.First?.ToString();
                }
            }

            // Series Description
            if (dicomJson.TryGetValue("0008103E", out var seriesDescriptionToken))
            {
                dto.SeriesDescription = seriesDescriptionToken["Value"]?.First?.ToString();
            }

            // Series Instance UID
            if (dicomJson.TryGetValue("0020000E", out var seriesInstanceUidToken))
            {
                dto.SeriesInstanceUID = seriesInstanceUidToken["Value"]?.First?.ToString();
            }

            // Series Number
            if (dicomJson.TryGetValue("00200010", out var seriesNumberToken))
            {
                int.TryParse(seriesNumberToken["Value"]?.First?.ToString(), out var seriesNumber);
                dto.SeriesNumber = seriesNumber;
            }

            // Modality
            if (dicomJson.TryGetValue("00080060", out var modalityToken))
            {
                dto.Modality = modalityToken["Value"]?.First?.ToString();
            }

            // Series Date
            if (dicomJson.TryGetValue("00080021", out var seriesDateToken))
            {
                if (DateTime.TryParseExact(seriesDateToken["Value"]?.First?.ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var seriesDate))
                {
                    dto.SeriesDateTime = seriesDate;
                }
            }

            // Patient Position
            if (dicomJson.TryGetValue("00180050", out var patientPositionToken))
            {
                dto.PatientPosition = patientPositionToken["Value"]?.First?.ToString();
            }

            // Study Description
            if (dicomJson.TryGetValue("00081030", out var studyDescriptionToken))
            {
                dto.StudyDescription = studyDescriptionToken["Value"]?.First?.ToString();
            }

            // Study Date
            if (dicomJson.TryGetValue("00080020", out var studyDateToken))
            {
                if (DateTime.TryParseExact(studyDateToken["Value"]?.First?.ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var studyDate))
                {
                    dto.StudyDate = studyDate;
                }
            }

            // Study Time
            if (dicomJson.TryGetValue("00080030", out var studyTimeToken))
            {
                if (TimeSpan.TryParseExact(studyTimeToken["Value"]?.First?.ToString(), "hhmmss", null, out var studyTime))
                {
                    dto.StudyTime = studyTime;
                }
            }

            // Accession Number
            if (dicomJson.TryGetValue("00080050", out var accessionNumberToken))
            {
                dto.AccessionNumber = accessionNumberToken["Value"]?.First?.ToString();
            }

            // Institution Name
            if (dicomJson.TryGetValue("00080080", out var institutionNameToken))
            {
                dto.InstitutionName = institutionNameToken["Value"]?.First?.ToString();
            }

            // Patient's Name
            if (dicomJson.TryGetValue("00100010", out var patientNameToken))
            {
                dto.PatientName = patientNameToken["Value"]?.First?.ToString();
            }

            // Patient's Sex
            if (dicomJson.TryGetValue("00100040", out var patientSexToken))
            {
                dto.PatientSex = patientSexToken["Value"]?.First?.ToString();
            }

            // Patient's Birth Date
            if (dicomJson.TryGetValue("00100030", out var patientBirthDateToken))
            {
                if (DateTime.TryParseExact(patientBirthDateToken["Value"]?.First?.ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var patientBirthDate))
                {
                    dto.PatientBirthDate = patientBirthDate;
                }
            }

            // Study Instance UID
            if (dicomJson.TryGetValue("0020000D", out var studyInstanceUidToken))
            {
                dto.StudyInstanceUID = studyInstanceUidToken["Value"]?.First?.ToString();
            }

            // Instance Number
            if (dicomJson.TryGetValue("00200011", out var instanceNumberToken))
            {
                int.TryParse(instanceNumberToken["Value"]?.First?.ToString(), out var instanceNumber);
                dto.InstanceNumber = instanceNumber;
            }

            // Rows
            if (dicomJson.TryGetValue("00280010", out var rowsToken))
            {
                int.TryParse(rowsToken["Value"]?.First?.ToString(), out var rows);
                dto.Rows = rows;
            }

            // Columns
            if (dicomJson.TryGetValue("00280011", out var columnsToken))
            {
                int.TryParse(columnsToken["Value"]?.First?.ToString(), out var columns);
                dto.Columns = columns;
            }

            // Pixel Spacing
            if (dicomJson.TryGetValue("00280030", out var pixelSpacingToken))
            {
                dto.PixelSpacing = pixelSpacingToken["Value"]?.First?.ToString();
            }

            // Image Position (Patient)
            if (dicomJson.TryGetValue("00200032", out var imagePositionToken))
            {
                dto.ImagePositionPatient = imagePositionToken["Value"]?.First?.ToString();
            }

            // Image Orientation (Patient)
            if (dicomJson.TryGetValue("00200037", out var imageOrientationToken))
            {
                dto.ImageOrientationPatient = imageOrientationToken["Value"]?.First?.ToString();
            }

            // Body Part Examined
            if (dicomJson.TryGetValue("00082120", out var bodyPartExaminedToken))
            {
                dto.BodyPartExamined = bodyPartExaminedToken["Value"]?.First?.ToString();
            }

            // Photometric Interpretation
            if (dicomJson.TryGetValue("00280004", out var photometricInterpretationToken))
            {
                dto.PhotometricInterpretation = photometricInterpretationToken["Value"]?.First?.ToString();
            }

            // -- Transaction -- //
            // Transaction UID
            if (dicomJson.TryGetValue("00081195", out var transactionUIDToken))
            {
                dto.TransactionUID = transactionUIDToken["Value"]?.First?.ToString();
            }

            // Transaction Status
            if (dicomJson.TryGetValue("00080417", out var transactionStatusToken))
            {
                dto.TransactionStatus = transactionStatusToken["Value"]?.First?.ToString();
            }
            

            return dto;
        }

    }
}
