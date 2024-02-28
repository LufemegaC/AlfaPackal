using AutoMapper;
using FellowOakDicom;
using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service.IService;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class DicomMapping
    {
        private readonly IMapper _mapper;
        private readonly IEstudioService _estudioService;

        public DicomMapping(IMapper mapper, IEstudioService estudioService)
        {
            _mapper = mapper;
            _estudioService = estudioService;
        }

        public async Task<APIResponse> MapAndSaveEstudio(DicomDataset dataset)
        {
            EstudioCreateDto estudioDto = _mapper.Map<EstudioCreateDto>(dataset);
            var response = await _estudioService.Crear<APIResponse>(estudioDto);
            return response; // Devuelve el DTO mapeado y quizás modificado
           
        }

    }
}
