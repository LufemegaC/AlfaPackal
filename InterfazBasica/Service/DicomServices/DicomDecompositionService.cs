using AutoMapper;
using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica_DCStore.Service.IDicomService;
using System.Reflection;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class DicomDecompositionService : IDicomDecompositionService
    {
        private readonly IMapper _mapper;

        public DicomDecompositionService(IMapper mapper)
        {
            _mapper = mapper;  
        }

        public void AnonymizeDicomFile(DicomFile dicomFile)
        {
            throw new NotImplementedException();
        }

        public void ConvertDicomToFile(DicomFile dicomFile, string outputPath, DicomTransferSyntax transferSyntax)
        {
            throw new NotImplementedException();
        }
        public Task<PacienteCreateDto> DecomposeDicomToPaciente(Dictionary<DicomTag, object> metadata)
        {
            var pacienteCreate = _mapper.Map<PacienteCreateDto>(metadata);
            return Task.FromResult(pacienteCreate);
        }
        public Task<EstudioCreateDto> DecomposeDicomToEstudio(Dictionary<DicomTag, object> metadata)
        {
            var estudioCreate = _mapper.Map<EstudioCreateDto>(metadata);
            //Si no tiene numero de acceso asigno uno
            if (string.IsNullOrWhiteSpace(estudioCreate.AccessionNumber))
            {
                estudioCreate.AccessionNumber = GenerarAccessionNumber();
            }
            return Task.FromResult(estudioCreate);
        }
        public Task<SerieCreateDto> DecomposeDicomToSerie(Dictionary<DicomTag, object> metadata)
        {
            var pacienteCreate = _mapper.Map<SerieCreateDto>(metadata);
            return Task.FromResult(pacienteCreate);
        }

        public Task<ImagenCreateDto> DecomposeDicomToImagen(Dictionary<DicomTag, object> metadata)
        {
            var imagenCreate = _mapper.Map<ImagenCreateDto>(metadata);
            return Task.FromResult(imagenCreate);
        }
       
        public IEnumerable<byte[]> ExtractImageFrames(DicomFile dicomFile)
        {
            throw new NotImplementedException();
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

        public static string GenerarAccessionNumber()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
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
        public string GetDicomElementAsString(DicomDataset dicomDataset, DicomTag dicomTag)
        {
            throw new NotImplementedException();
        }

        public DateTime? GetStudyDateTime(DicomDataset dicomDataset)
        {
            throw new NotImplementedException();
        }

        //public void UpdatePatientData(DicomFile dicomFile, string patientName, string patientId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
