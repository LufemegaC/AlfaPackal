using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto.Pacs;
using Api_PACsServer.Modelos.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Services.IService.Pacs;
using AutoMapper;
using FellowOakDicom;


namespace Api_PACsServer.Services
{
    public class EstudioService : IEstudioService
    {
        private readonly IEstudioRepositorio _estudioRepo;
        private readonly IEstudioCargaRepositorio _estudioCargaRepo;
        private readonly IMapper _mapper;

        public EstudioService(IEstudioRepositorio estudioRepo, IEstudioCargaRepositorio estudioCargaRepo, IMapper mapper)
        {
            _estudioRepo = estudioRepo;
            _estudioCargaRepo = estudioCargaRepo;
            _mapper = mapper;
        }

        public async Task<EstudioDto> Crear(EstudioCreateDto createDto)
        {
            var studyInstanceUID = createDto.StudyInstanceUID;
            var SizeFile = createDto.TotalFileSizeMB;
            // Validación específica para StudyInstanceUID
            if (!DicomUID.IsValidUid(studyInstanceUID))
                throw new ArgumentException("El formato de la instancia UID no es válido.");
            if (await _estudioRepo.ExistByInstanceUID(studyInstanceUID))
                throw new InvalidOperationException("Ya existe una imagen con el mismo SOPInstanceUID.");
            // Validación de StudyDate
            if (createDto.StudyDate > DateTime.Today)
                throw new ArgumentException("El formato de la instancia UID no es válido.");
            //valido Institucion emisora
            if (createDto.InstitutionId == 0)
                throw new ArgumentException("el Id de la institucion es obligatorio.");
            // Registro de entidad de serie
            var estudio = _mapper.Map<Estudio>(createDto);
            await _estudioRepo.Crear(estudio);
            await _estudioRepo.Grabar();
            // Registro de informacion de carga
            var estudioCarga = MapSerieToCarga(estudio, SizeFile);
            await _estudioCargaRepo.Crear(estudioCarga);
            await _estudioCargaRepo.Grabar();
            //Resultado
            var estudioDto = _mapper.Map<EstudioDto>(estudio);
            estudioDto.TotalFileSizeMB = estudioCarga.TotalFileSizeMB;
            return estudioDto;
        }
        public async Task<EstudioDto> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            var estudio = await _estudioRepo.Obtener(v => v.PACS_EstudioID == id);
            if (estudio == null)
                throw new KeyNotFoundException("Estudio no encontrado.");
            return _mapper.Map<EstudioDto>(estudio);
        }

        private static EstudioCarga MapSerieToCarga(Estudio estudio, decimal totalSizeFile)
        // Pendiente delegar a Servicio o mapeo
        {
            var estudioCarga = new EstudioCarga
            {
                PACS_EstudioID = estudio.PACS_EstudioID,
                NumberOfFiles = 1,
                NumberOfSeries = 1,
                TotalFileSizeMB = totalSizeFile,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
            return estudioCarga;
        }

        public async Task<EstudioDto> GetByUID(string InstanceUID)
        {
            //Valido formato con fo-dicom
            if (!DicomUID.IsValidUid(InstanceUID))
                throw new ArgumentException("El formato de la instancia UID no es válido.");
            var estudio = await _estudioRepo.Obtener(v => v.StudyInstanceUID == InstanceUID);
            if (estudio == null)
                throw new KeyNotFoundException("Estudio no encontrado.");
            return _mapper.Map<EstudioDto>(estudio);               
        }
        public EstudioDto MapToDto(Estudio estudio)
        {
            return _mapper.Map<EstudioDto>(estudio);
        }
    }
}
