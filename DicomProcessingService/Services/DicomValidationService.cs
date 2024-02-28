using DicomProcessingService.Services.Interfaces;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using System.Text.RegularExpressions;

namespace DicomProcessingService.Services
{
    public class DicomValidationService: IDicomValidationService
    {
        // Patrón de expresión regular para UIDs DICOM
        private static readonly Regex DicomUidRegex = new Regex(@"^\d+(\.\d+)+$");

        // Método para validar UIDs DICOM
        public bool IsValidDicomUid(string uid)
        {
            return !string.IsNullOrWhiteSpace(uid) && uid.Length <= 64 && DicomUidRegex.IsMatch(uid);
        }

        // Genera UID para movimientos de pruebas 
        public string GenerateUid()
        {
            return DicomUIDGenerator.GenerateDerivedFromUUID().UID;
        }

        // Método para validar si el conjunto de datos DICOM cumple con ciertos criterios
        public bool ValidateDataset(DicomDataset dataset)
        {
            // Realizar validaciones específicas, como verificar la presencia y el formato de ciertos tags requeridos
            // Retornar true si todas las validaciones pasan, false de lo contrario
            /*
            // Example: Check if mandatory tags such as Patient ID and Study Instance UID are present
                 var mandatoryTags = new List<DicomTag>
                 {
                     DicomTag.PatientID,
                     DicomTag.StudyInstanceUID
                     // Add more mandatory tags as per DICOM IOD requirements
                 };

                 foreach (var tag in mandatoryTags)
                 {
                     if (!dataset.Contains(tag) || string.IsNullOrWhiteSpace(dataset.GetSingleValueOrDefault<string>(tag, string.Empty)))
                     {
                         // Log the missing mandatory tag
                         return false;
                     }
                 }

                 // Perform other dataset consistency checks as per DICOM IOD
                 // ...

             */
            return true;
        }

        // Método para validar la consistencia del conjunto de datos con el estándar DICOM
        public bool ValidateAgainstStandard(DicomDataset dataset)

        {
            // Implementar reglas de validación basadas en el estándar DICOM
            // Retornar true si el dataset es consistente con el estándar, false de lo contrario
            /*
            // Example: Check if the dataset conforms to a particular DICOM profile or standard
            // This could involve checking SOP Class UIDs, Transfer Syntaxes, etc.

                var sopClassUid = dataset.GetSingleValueOrDefault(DicomTag.SOPClassUID, string.Empty);
                // Define the list of SOP Class UIDs that your application needs to support
                var supportedSopClassUids = new HashSet<string>
                {
                    // Add supported SOP Class UIDs
                };

                if (!supportedSopClassUids.Contains(sopClassUid))
                {
                    // Log unsupported SOP Class UID
                    return false;
                }

                // Perform other standard compliance checks
                // ...
            
            */
            return true;
        }

        // Método para validar la integridad de la imagen DICOM
        public bool ValidateImageIntegrity(DicomFile dicomFile)
        {
            // Verificar la integridad de la imagen, como la correcta decodificación de los frames
            // Retornar true si la imagen es válida, false de lo contrario
            /*
            try
            {
                // Attempt to decode the pixel data
                var pixelData = DicomPixelData.Create(dicomFile.Dataset);
                for (int i = 0; i < pixelData.NumberOfFrames; i++)
                {
                    var frame = pixelData.GetFrame(i);
                    // You could attempt to perform an operation on the frame to ensure it's valid
                    // For example, verify the frame size or checksum if available
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                return false;
            }*/
            // Perform other image integrity checks
            // ...

            return true;
        }
    }
}
