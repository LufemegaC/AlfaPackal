using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica.Models;
using InterfazBasica_DCStore.Models;
using InterfazBasica_DCStore.Models.Dtos.MainEntities;
using Newtonsoft.Json.Linq;
using System.Net;

namespace InterfazBasica_DCStore.Utilities
{
    public class DicomUtility
    {
        // Método para validar UIDs DICOM 
        public static string IssuerOfPatientIDValue { get; } = "PackalPACS";
        private static int counter = 0;
        private static readonly object lockObject = new object();

        // *** Definición de Constantes DicomStatus Personalizadas*** //
        // StudyCreate
        public static readonly DicomStatus StudyCreateInvalidPatientName = new DicomStatus("A901", DicomState.Failure, "Invalid or missing patient name");
        public static readonly DicomStatus StudyCreateInvalidStudyUID = new DicomStatus("A902", DicomState.Failure, "Invalid or missing StudyInstanceUID");
        public static readonly DicomStatus StudyCreateInvalidModality = new DicomStatus("A903", DicomState.Failure, "Invalid or missing Modality");
        public static readonly DicomStatus StudyCreateInvalidStudyDate = new DicomStatus("A904", DicomState.Failure, "Study date is in the future");
        public static readonly DicomStatus StudyCreateInvalidAccessionNumber = new DicomStatus("A905", DicomState.Failure, "AccessionNumber exceeds the maximum length");
        // SerieCreate
        public static readonly DicomStatus SerieCreateInvalidSeriesUID = new DicomStatus("A906", DicomState.Failure, "Invalid or missing SeriesInstanceUID");
        public static readonly DicomStatus SerieCreateInvalidModality = new DicomStatus("A907", DicomState.Failure, "Invalid or missing Modality");
        public static readonly DicomStatus SerieCreateInvalidDate = new DicomStatus("A908", DicomState.Failure, "Series date is in the future");
        // InstanceCreate
        public static readonly DicomStatus InstanceCreateInvalidSOPInstanceUID = new DicomStatus("A909", DicomState.Failure, "Invalid or missing SOPInstanceUID");
        public static readonly DicomStatus InstanceCreateInvalidImageNumber = new DicomStatus("A90A", DicomState.Failure, "ImageNumber is invalid");
        public static readonly DicomStatus InstanceCreateInvalidPhotometricInterpretation = new DicomStatus("A90B", DicomState.Warning, "Photometric interpretation information is missing");
        public static readonly DicomStatus InstanceCreateInvalidDimension = new DicomStatus("A90C", DicomState.Failure, "Invalid dimension information");
        // Internal Error
        public static readonly DicomStatus ServerInternalError = new DicomStatus("A90D", DicomState.Failure, "Internal error");
        // A90E APARTO LUGAR PARA Exceptions
        //public static readonly DicomStatus ServerInternalError = new DicomStatus("A90E", DicomState.Failure, "Internal error");

        public static string PatientIDGenerator()
        // 17/03/24.- Funcion estatica para generar PatienteID
        {
            // Prefix de emisor
            string prefix = "Packal";
            // Timestamp: Usar el timestamp asegura un componente único en la generación del ID
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            // RandomSuffix: Agregar un sufijo aleatorio para reducir aún más la posibilidad de colisiones
            Random random = new Random();
            int randomSuffix = random.Next(1000, 9999); // Genera un número aleatorio entre 1000 y 9999
            // Combinar los componentes para formar el PatientID
            string patientID = $"{prefix}-{timestamp}-{randomSuffix}";
            // Asegurar que el PatientID no exceda los 64 caracteres permitidos por el estándar DICOM
            if (patientID.Length > 64)
            {
                throw new InvalidOperationException("El PatientID generado excede la longitud máxima permitida de 64 caracteres.");
            }
            return patientID;
        }

        public static string GenerateAccessionNumber()
        {
            lock (lockObject)
            {
                // Reinicia el contador cada día (opcional)
                if (counter >= 9999) counter = 0;

                // Incrementa el contador
                counter++;

                // Formato: AÑOMESDIA-CONTADOR (p.ej., 20210323-0001)
                return $"{DateTime.Now:yyyyMMdd}-{counter:0000}";
            }
        }
        
        public static bool ValidateUID(string uid)
        {
            return DicomUID.IsValidUid(uid);
        }

        public static DicomStatus TranslateApiResponseToDicomStatus(APIResponse apiResponse)
        {
            // Caso de éxito
            if (apiResponse.StatusCode == HttpStatusCode.OK && apiResponse.IsSuccessful)
            {
                return DicomStatus.Success;
            }

            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return DicomStatus.InvalidArgumentValue;
                case HttpStatusCode.NotFound:
                    return DicomStatus.NoSuchObjectInstance;
                case HttpStatusCode.InternalServerError:
                    return DicomStatus.ProcessingFailure;
                case HttpStatusCode.ServiceUnavailable:
                    return DicomStatus.ResourceLimitation;
                default:
                    // Un mapeo genérico para otros casos
                    return DicomStatus.ProcessingFailure;
            }
        }

        // Método para inicializar los DicomStatus personalizados
        public static void InitializeCustomDicomStatuses()
        {

            DicomStatus.AddKnownDicomStatuses(new List<DicomStatus>
            {
                // Agregando las constantes definidas al diccionario _entries
                StudyCreateInvalidPatientName,
                StudyCreateInvalidStudyUID,
                StudyCreateInvalidModality,
                StudyCreateInvalidStudyDate,
                StudyCreateInvalidAccessionNumber,
                SerieCreateInvalidSeriesUID,
                SerieCreateInvalidModality,
                SerieCreateInvalidDate,
                InstanceCreateInvalidSOPInstanceUID,
                InstanceCreateInvalidImageNumber,
                InstanceCreateInvalidPhotometricInterpretation,
                InstanceCreateInvalidDimension,
                ServerInternalError

            });

        }


        /// <summary>
        /// Converts a MainEntitiesCreateDto object to a DICOM JSON structure based on a predefined template.
        /// This method follows the DICOM STOW-RS standard to facilitate storing DICOM instances.
        /// Each tag in the DICOM JSON is documented with its corresponding meaning.
        /// </summary>
        /// <param name="dto">The DTO containing the metadata for the DICOM instance.</param>
        /// <returns>A JObject representing the DICOM JSON structure.</returns>
        public static JObject ConvertDtoToDicomJson(MetadataDto dto)
        {
            var dicomJson = new JObject();

            // Add fields only if they are valid (non-null, non-empty, or correctly formatted)

            // SOP Class UID
            if (!string.IsNullOrEmpty(dto.SOPClassUID))
            {
                dicomJson["00020010"] = new JObject
                {
                    ["vr"] = "UI",
                    ["Value"] = new JArray(dto.SOPClassUID) // Unique Identifier for the Service-Object Pair (SOP) Class
                };
            }

            // Referenced Series Sequence
            if (!string.IsNullOrEmpty(dto.SOPInstanceUID) && !string.IsNullOrEmpty(dto.SeriesInstanceUID))
            {
                dicomJson["00081199"] = new JObject
                {
                    ["vr"] = "SQ",
                    ["Value"] = new JArray(new JObject
                    {
                        // SOP Class UID
                        ["00080016"] = new JObject
                        {
                            ["vr"] = "UI",
                            ["Value"] = new JArray(dto.SOPInstanceUID) // Unique Identifier for the SOP Class
                        },
                        // SOP Instance UID
                        ["00080018"] = new JObject
                        {
                            ["vr"] = "UI",
                            ["Value"] = new JArray(dto.SeriesInstanceUID) // Unique Identifier for the SOP Instance
                        }
                    })
                };
            }

            // Series Description
            if (!string.IsNullOrEmpty(dto.SeriesDescription))
            {
                dicomJson["0008103E"] = new JObject
                {
                    ["vr"] = "LO",
                    ["Value"] = new JArray(dto.SeriesDescription) // Description of the Series
                };
            }

            // Series Instance UID
            if (!string.IsNullOrEmpty(dto.SeriesInstanceUID))
            {
                dicomJson["0020000E"] = new JObject
                {
                    ["vr"] = "UI",
                    ["Value"] = new JArray(dto.SeriesInstanceUID) // Unique Identifier for the Series
                };
            }

            // Series Number
            if (dto.SeriesNumber.HasValue)
            {
                dicomJson["00200010"] = new JObject
                {
                    ["vr"] = "IS",
                    ["Value"] = new JArray(dto.SeriesNumber.Value.ToString()) // Number that identifies the Series
                };
            }

            // Modality
            if (!string.IsNullOrEmpty(dto.Modality))
            {
                dicomJson["00080060"] = new JObject
                {
                    ["vr"] = "CS",
                    ["Value"] = new JArray(dto.Modality) // Type of equipment that created the Series
                };
            }

            // Series Date
            if (dto.SeriesDateTime.HasValue && dto.SeriesDateTime.Value != DateTime.MinValue)
            {
                dicomJson["00080021"] = new JObject
                {
                    ["vr"] = "DA",
                    ["Value"] = new JArray(dto.SeriesDateTime.Value.ToString("yyyyMMdd")) // Date the Series started
                };
            }

            // Patient Position
            if (!string.IsNullOrEmpty(dto.PatientPosition))
            {
                dicomJson["00180050"] = new JObject
                {
                    ["vr"] = "DS",
                    ["Value"] = new JArray(dto.PatientPosition) // Position of the patient during the imaging
                };
            }

            // Study Description
            if (!string.IsNullOrEmpty(dto.StudyDescription))
            {
                dicomJson["00081030"] = new JObject
                {
                    ["vr"] = "LO",
                    ["Value"] = new JArray(dto.StudyDescription) // Description of the Study
                };
            }

            // Study Date
            if (dto.StudyDate != DateTime.MinValue)
            {
                dicomJson["00080020"] = new JObject
                {
                    ["vr"] = "DA",
                    ["Value"] = new JArray(dto.StudyDate.ToString("yyyyMMdd")) // Date the Study started
                };
            }

            // Study Time
            if (dto.StudyTime.HasValue)
            {
                var studyTimeValue = dto.StudyTime.Value;
                try
                {
                    var formattedStudyTime = new TimeSpan(studyTimeValue.Hours, studyTimeValue.Minutes, studyTimeValue.Seconds).ToString("hhmmss");
                    dicomJson["00080030"] = new JObject
                    {
                        ["vr"] = "TM",
                        ["Value"] = new JArray(formattedStudyTime) // Time the Study started
                    };
                }
                catch (FormatException)
                {
                    // Log or handle the incorrect format issue if needed
                }
            }



            // Accession Number
            if (!string.IsNullOrEmpty(dto.AccessionNumber))
            {
                dicomJson["00080050"] = new JObject
                {
                    ["vr"] = "SH",
                    ["Value"] = new JArray(dto.AccessionNumber) // Identifier for the Study
                };
            }

            // Institution Name
            if (!string.IsNullOrEmpty(dto.InstitutionName))
            {
                dicomJson["00080080"] = new JObject
                {
                    ["vr"] = "LO",
                    ["Value"] = new JArray(dto.InstitutionName) // Name of the institution where the Study was performed
                };
            }

            // Patient's Name
            if (!string.IsNullOrEmpty(dto.PatientName))
            {
                dicomJson["00100010"] = new JObject
                {
                    ["vr"] = "PN",
                    ["Value"] = new JArray(dto.PatientName) // Full name of the patient
                };
            }

            // Patient's Sex
            if (!string.IsNullOrEmpty(dto.PatientSex))
            {
                dicomJson["00100040"] = new JObject
                {
                    ["vr"] = "CS",
                    ["Value"] = new JArray(dto.PatientSex) // Sex of the patient
                };
            }

            // Patient's Birth Date
            if (dto.PatientBirthDate.HasValue && dto.PatientBirthDate.Value != DateTime.MinValue)
            {
                dicomJson["00100030"] = new JObject
                {
                    ["vr"] = "DA",
                    ["Value"] = new JArray(dto.PatientBirthDate.Value.ToString("yyyyMMdd")) // Birth date of the patient
                };
            }

            // Study Instance UID
            if (!string.IsNullOrEmpty(dto.StudyInstanceUID))
            {
                dicomJson["0020000D"] = new JObject
                {
                    ["vr"] = "UI",
                    ["Value"] = new JArray(dto.StudyInstanceUID) // Unique Identifier for the Study
                };
            }

            // Instance Number
            if (dto.InstanceNumber > 0)
            {
                dicomJson["00200011"] = new JObject
                {
                    ["vr"] = "IS",
                    ["Value"] = new JArray(dto.InstanceNumber.ToString()) // Number that identifies the Instance within the Series
                };
            }

            // Rows
            if (dto.Rows > 0)
            {
                dicomJson["00280010"] = new JObject
                {
                    ["vr"] = "US",
                    ["Value"] = new JArray(dto.Rows.ToString()) // Number of rows in the image
                };
            }

            // Columns
            if (dto.Columns > 0)
            {
                dicomJson["00280011"] = new JObject
                {
                    ["vr"] = "US",
                    ["Value"] = new JArray(dto.Columns.ToString()) // Number of columns in the image
                };
            }

            // Pixel Spacing
            if (!string.IsNullOrEmpty(dto.PixelSpacing))
            {
                dicomJson["00280030"] = new JObject
                {
                    ["vr"] = "DS",
                    ["Value"] = new JArray(dto.PixelSpacing) // Physical distance between the centers of adjacent pixels
                };
            }

            // Image Position (Patient)
            if (!string.IsNullOrEmpty(dto.ImagePositionPatient))
            {
                dicomJson["00200032"] = new JObject
                {
                    ["vr"] = "DS",
                    ["Value"] = new JArray(dto.ImagePositionPatient) // Position of the image in the patient
                };
            }

            // Image Orientation (Patient)
            if (!string.IsNullOrEmpty(dto.ImageOrientationPatient))
            {
                dicomJson["00200037"] = new JObject
                {
                    ["vr"] = "DS",
                    ["Value"] = new JArray(dto.ImageOrientationPatient) // Orientation of the image in the patient
                };
            }

            // Body Part Examined
            if (!string.IsNullOrEmpty(dto.BodyPartExamined))
            {
                dicomJson["00082120"] = new JObject
                {
                    ["vr"] = "SH",
                    ["Value"] = new JArray(dto.BodyPartExamined) // Body part that was examined
                };
            }

            // Photometric Interpretation
            if (!string.IsNullOrEmpty(dto.PhotometricInterpretation))
            {
                dicomJson["00280004"] = new JObject
                {
                    ["vr"] = "CS",
                    ["Value"] = new JArray(dto.PhotometricInterpretation) // Specifies the intended interpretation of the pixel data
                };
            }
            // -- Transaction -- //
            // Transaction UID
            if (!string.IsNullOrEmpty(dto.TransactionUID))
            {
                dicomJson["00081195"] = new JObject
                {
                    ["vr"] = "UI",
                    ["Value"] = new JArray(dto.TransactionUID) // Unique Identifier for the Service-Object Pair (SOP) Class
                };
            }
            // Transaction Status
            if (!string.IsNullOrEmpty(dto.TransactionStatus))
            {
                dicomJson["00080417"] = new JObject
                {
                    ["vr"] = "CS",
                    ["Value"] = new JArray(dto.TransactionStatus) // Unique Identifier for the Service-Object Pair (SOP) Class
                };
            }

            // Transaction Status (0008,0418) VR=LT VM=1
            if (!string.IsNullOrEmpty(dto.TransactionStatusComment))
            {
                dicomJson["00080418"] = new JObject
                {
                    ["vr"] = "LT",
                    ["Value"] = new JArray(dto.TransactionStatusComment) // Unique Identifier for the Service-Object Pair (SOP) Class
                };
            }

            return dicomJson;
        }
    }
}

