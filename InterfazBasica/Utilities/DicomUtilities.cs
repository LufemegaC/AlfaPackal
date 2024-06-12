using InterfazBasica_DCStore.Models;

namespace InterfazBasica_DCStore.Utilities
{
    public class DicomUtilities
    {
        // Método para validar UIDs DICOM
        public static string IssuerOfPatientIDValue { get; } = "PackalPACS";
        private static int counter = 0;
        private static readonly object lockObject = new object();

        // 
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
 
    }

}
