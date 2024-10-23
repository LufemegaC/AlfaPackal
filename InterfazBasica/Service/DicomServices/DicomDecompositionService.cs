using AutoMapper;
using FellowOakDicom;
using InterfazBasica_DCStore.Models.Dtos.MainEntities;
using InterfazBasica_DCStore.Service.IService.Dicom;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class DicomDecompositionService : IDicomDecompositionService
    {
        private readonly IMapper _mapper;
        public DicomDecompositionService(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
        }

        public Dictionary<DicomTag, object> ExtractMetadata(DicomDataset dicomDataset)
        // Método para extraer los metadatos de un DicomDataset.
        // Retorna un diccionario con las etiquetas DICOM y sus valores asociados.
        {
            try
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
            catch (Exception ex)
            {
                return null;// Diccionario lleno de metadatos.
            }
        }

        public MetadataDto DicomDictionaryToCreateEntities(Dictionary<DicomTag, object> metadata)
        {
            return _mapper.Map<MetadataDto>(metadata);

        }

        private static object ExtractElementValue(DicomElement element)
        {
            // Método para extraer el valor de un DicomElement.
            if (element.Count > 1)
            {
                return element.Get<string[]>();
            }
            // Si el elemento tiene un solo valor, extrae ese valor.
            else if (element.Count == 1)
            {
                return element.Get<string>();
            }
            // Si el elemento no tiene valores, retorna null o una cadena vacía.
            return null;
        }

        public decimal GetFileSizeInMB(DicomFile dicomFile)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    // Guarda el archivo en el MemoryStream
                    dicomFile.Save(memoryStream);
                    // El tamaño del archivo DICOM en bytes
                    long fileSizeInBytes = memoryStream.Length;
                    decimal fileSizeInMB = (decimal)fileSizeInBytes / (1024 * 1024);
                    return fileSizeInMB;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or throw it, depending on your logging strategy
                Console.WriteLine($"Error al obtener el tamaño del archivo: {ex.Message}");
                // Puedes lanzar la excepción nuevamente o manejarla según sea necesario
                throw;
            }
        }
    }
}
