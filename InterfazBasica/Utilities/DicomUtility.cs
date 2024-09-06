using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica.Models;
using InterfazBasica_DCStore.Models;
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
    }

}
