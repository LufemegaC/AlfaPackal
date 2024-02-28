using AutoMapper;
using DicomProcessingService.Services.Interfaces;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Imaging.Codec;

namespace DicomProcessingService.Services
{
    public class DicomDecompositionService : IDicomDecompositionService
    {
        private readonly IMapper _mapper;

        public DicomDecompositionService(IMapper mapper)
        {
            _mapper = mapper;
        }



        public Dictionary<DicomTag, object> ExtractMetadata(DicomDataset dicomDataset)   
            
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


        public void AnonymizeDicomFile(DicomFile dicomFile)
        {
            dicomFile.Dataset.Remove(DicomTag.PatientName);
            dicomFile.Dataset.Remove(DicomTag.PatientID);
            // Añadir o remover otros tags según sea necesario para la anonimización.
            dicomFile.Save("Anonymized.dcm");
        }


        // Método para leer y extraer frames de imagen de un archivo DICOM
        public IEnumerable<byte[]> ExtractImageFrames(DicomFile dicomFile)
        {
            var pixelData = DicomPixelData.Create(dicomFile.Dataset);
            List<byte[]> frames = new List<byte[]>();

            for (int i = 0; i < pixelData.NumberOfFrames; i++)
            {
                var frame = pixelData.GetFrame(i);
                frames.Add(frame.Data);
            }

            return frames;
        }


        public string GetDicomElementAsString(DicomDataset dicomDataset, DicomTag dicomTag)
        {
            return dicomDataset.GetSingleValue<string>(dicomTag);
        }

        public DateTime? GetStudyDateTime(DicomDataset dicomDataset)
        {
            if (dicomDataset.TryGetSingleValue(DicomTag.StudyDate, out string date) &&
            dicomDataset.TryGetSingleValue(DicomTag.StudyTime, out string time))
            {
                if (DateTime.TryParseExact(date + time, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            return null;
        }
    

        public void ConvertDicomToFile(DicomFile dicomFile, string outputPath, DicomTransferSyntax transferSyntax)
        {
            var transcoder = new DicomTranscoder(dicomFile.Dataset.InternalTransferSyntax, transferSyntax);
            var transcodedDataset = transcoder.Transcode(dicomFile.Dataset);
            var transcodedFile = new DicomFile(transcodedDataset);
            transcodedFile.Save(outputPath);
        }

        public void UpdatePatientData(DicomFile dicomFile, string patientName, string patientId)
        {
            dicomFile.Dataset.AddOrUpdate(DicomTag.PatientName, patientName);
            dicomFile.Dataset.AddOrUpdate(DicomTag.PatientID, patientId);
            // Añadir o actualizar otros datos del paciente según sea necesario
        }

    }
}
