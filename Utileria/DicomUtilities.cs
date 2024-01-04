using FellowOakDicom;
using FellowOakDicom.Imaging.Codec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utileria
{
    public class DicomUtilities
    {
        // Patrón de expresión regular para UIDs DICOM
        private static readonly Regex DicomUidRegex = new Regex(@"^\d+(\.\d+)+$");

        // Método para validar UIDs DICOM
        public static  bool IsValidDicomUid(string uid)
        {
            return !string.IsNullOrWhiteSpace(uid) && uid.Length <= 64 && DicomUidRegex.IsMatch(uid);
        }
        // Genera UID para movimientos de pruebas 
        public static string GenerateUid()
        {
            return DicomUIDGenerator.GenerateDerivedFromUUID().UID;
        }
        // 
        public static DicomFile ConvertToTransferSyntax(DicomFile dicomFile, DicomTransferSyntax transferSyntax)
        {
            var transcodedFile = dicomFile.Clone(transferSyntax);
            return transcodedFile;
        }


    
    public static Dictionary<DicomTag, object> ExtractMetadata(DicomDataset dicomDataset)
    // Método para extraer los metadatos de un DicomDataset.
    // Retorna un diccionario con las etiquetas DICOM y sus valores asociados.
    {
            // Inicializamos un diccionario para almacenar los metadatos.
            var metadata = new Dictionary<DicomTag, object>();        
            foreach (DicomItem item in dicomDataset)  // Iteramos sobre cada DicomItem en el DicomDataset.
            {
                switch (item) // Utilizamos un switch para determinar el tipo de cada DicomItem.
                {
                    case DicomElement element: // Elemento individual.
                        metadata[item.Tag] = ExtractElementValue(element);// Extraemos el valor y lo añadimos al diccionario.
                        break;
                    case DicomSequence sequence:// (una secuencia de DicomDataset).
                        metadata[item.Tag] = sequence.Items.Select(ExtractMetadata).ToList(); // extraemos recursivamente los metadatos de sus ítems y los añadimos al diccionario.
                        break;
                        //Más casos 
                }
            } 
            return metadata;// Diccionario lleno de metadatos.
        }

        // Método para extraer el valor de un DicomElement.
        // Esta función maneja la extracción de valores de elementos individuales.
        private static object ExtractElementValue(DicomElement element)
        {
            // Si el elemento tiene más de un valor (es decir, su multiplicidad de valor es mayor que 1).
            if (element.Count > 1)
            {
                // Extraemos todos los valores en forma de arreglo de strings.
                return element.Get<string[]>(-1);
            }

            // Si el elemento tiene un solo valor, extraemos ese único valor como string.
            return element.Get<string>(0);
        }


        public static void AnonymizeDicomFile(DicomFile dicomFile)
        {
            dicomFile.Dataset.Remove(DicomTag.PatientName);
            dicomFile.Dataset.Remove(DicomTag.PatientID);
            // Añadir o remover otros tags según sea necesario para la anonimización.
            dicomFile.Save("Anonymized.dcm");
        }
    }
}
