using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica.Models;
using InterfazBasica_DCStore.Models;
using InterfazBasica_DCStore.Models.Dtos.MainEntities;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Reflection;

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

        public static readonly DicomTag[] MetadataTags =
        {
            // SOP
            DicomTag.SOPClassUID,
            DicomTag.SOPInstanceUID,
            DicomTag.SeriesInstanceUID,
            DicomTag.StudyInstanceUID,
            DicomTag.TransferSyntaxUID,
        
            // Instance
            DicomTag.ImageComments,
            DicomTag.InstanceNumber,
            DicomTag.PhotometricInterpretation,
            DicomTag.Rows,
            DicomTag.Columns,
            DicomTag.PixelSpacing,
            DicomTag.NumberOfFrames,
            DicomTag.ImagePositionPatient,
            DicomTag.ImageOrientationPatient,
            DicomTag.BitsAllocated,
            DicomTag.SliceThickness,
            DicomTag.SliceLocation,
            DicomTag.InstanceCreationDate,
            DicomTag.InstanceCreationTime,
            DicomTag.ContentDate,
            DicomTag.ContentTime,
            DicomTag.AcquisitionDateTime, // new
        
            // Series
            DicomTag.SeriesDescription,
            DicomTag.SeriesNumber,
            DicomTag.Modality,
            //DicomTag.AcquisitionDateTime,
            DicomTag.PatientPosition,
            DicomTag.ProtocolName,
            DicomTag.SeriesDate,
            DicomTag.SeriesTime,
        
            // Study
            //DicomTag.StudyComments,
            DicomTag.StudyDescription,
            DicomTag.StudyDate,
            DicomTag.StudyTime,
            DicomTag.AccessionNumber,
            DicomTag.InstitutionName,
            DicomTag.BodyPartExamined,
        
            // Patient
            DicomTag.PatientName,
            DicomTag.PatientAge,
            DicomTag.PatientSex,
            DicomTag.PatientWeight,
            DicomTag.PatientBirthDate,
            DicomTag.IssuerOfPatientID,
        
            // Transaction (tags DICOM estándar si existen)
            DicomTag.TransactionUID,
            DicomTag.TransactionStatus,
            DicomTag.TransactionStatusComment
        };




        /// <summary>
        /// Converts a MetadataDto instance to a JSON+DICOM format.
        /// </summary>
        /// <param name="metadata">The MetadataDto instance to be converted.</param>
        /// <returns>A JSON string representing the DICOM data in JSON+DICOM format.</returns>
        public static string ConvertMetadataDtoToDicomJsonString(MetadataDto metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata), "The input metadata is null.");

            var dicomJson = new JObject();

            // Use reflection to iterate through the properties of MetadataDto
            foreach (var property in typeof(MetadataDto).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = property.GetValue(metadata);

                if (value != null) // Check if the property has a value
                {
                    // Attempt to get the corresponding DICOM Tag
                    var dicomTag = GetDicomTagFromPropertyName(property.Name);

                    if (dicomTag != null)
                    {
                        // Convert the value to the appropriate DICOM JSON format
                        var formattedValue = FormatDicomJsonValue(dicomTag, value);
                        // Formatear el DicomTag en formato hexadecimal sin paréntesis ni comas
                        var tagHex = $"{dicomTag.Group:X4}{dicomTag.Element:X4}";

                        // Agregar el tag al JSON principal
                        dicomJson[tagHex] = formattedValue;
                    }
                }
            }

            return dicomJson.ToString();
        }


        /// <summary>
        /// Gets the corresponding DICOM Tag for a given property name.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The corresponding DICOM Tag or null if not found.</returns>
        private static DicomTag? GetDicomTagFromPropertyName(string propertyName)
        {
            // Buscar en el DicomDictionary.Default que contiene todos los tags DICOM estándar.
            var entry = DicomDictionary.Default.FirstOrDefault(e =>
                e.Keyword.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            // Verifica si el tag fue encontrado y retorna el DicomTag correspondiente
            return entry?.Tag;
        }

        /// <summary>
        /// Formats a property value into the appropriate DICOM JSON format.
        /// </summary>
        /// <param name="dicomTag">The DICOM Tag associated with the property.</param>
        /// <param name="value">The value of the property.</param>
        /// <returns>A JObject representing the formatted DICOM JSON entry.</returns>
        private static JObject FormatDicomJsonValue(DicomTag dicomTag, object value)
        {

            // Obtiene las representaciones de valor (Value Representations) asociadas al DicomTag
            var vrs = dicomTag.DictionaryEntry.ValueRepresentations;

            // Usa el primer VR si hay más de uno
            string vr = vrs.Length > 0 ? vrs[0].Code : "UN"; // "UN" representa Unknown VR como fallback

            // Crear el array JSON basado en el valor proporcionado
            JArray jsonValue;
            if (value is IEnumerable<string> stringValues)
            {
                jsonValue = new JArray(stringValues);
            }
            else if (value is IEnumerable<object> objectValues)
            {
                jsonValue = new JArray(objectValues.Select(o => o.ToString()));
            }
            else
            {
                jsonValue = new JArray(value.ToString());
            }

            // Crear el objeto JSON que representa el formato DICOM JSON
            return new JObject
            {
                ["vr"] = vr,
                ["Value"] = jsonValue
            };
        }



    }
}

