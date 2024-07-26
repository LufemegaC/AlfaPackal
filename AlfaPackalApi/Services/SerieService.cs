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
    public class SerieService : ISerieService
    {
        private readonly ISerieRepositorio _serieRepo;
        private readonly IEstudioCargaRepositorio _estudiCargaRepo;
        private readonly ISerieCargaRepositorio _serieCargaRepo;
        private readonly IMapper _mapper;

        public SerieService(ISerieRepositorio serieRepo, ISerieCargaRepositorio serieCargaRepo,
                            IEstudioCargaRepositorio estudiCargaRepo, IMapper mapper)
        {
            _serieRepo = serieRepo;
            _serieCargaRepo = serieCargaRepo;
            _estudiCargaRepo = estudiCargaRepo;
            _mapper = mapper;
        }

        public async Task<SerieDto> Create(SerieCreateDto createDto)
        {

            if (!DicomUID.IsValidUid(createDto.SeriesInstanceUID))
                throw new ArgumentException("El formato de la instancia UID no es válido.");
            // Validación específica para StudyInstanceUID
            if (await _serieRepo.ExistByInstanceUID(createDto.SeriesInstanceUID))
                throw new InvalidOperationException("El formato de la instancia UID no es válido.");
            // Registro de entidad de serie
            var serie = _mapper.Map<Serie>(createDto);
            serie.CreationDate = DateTime.UtcNow;
            await _serieRepo.Crear(serie);
            // Registro de informacion de carga
            var serieCarga = MapSerieToCarga(serie, createDto.TotalFileSizeMB);
            await _serieCargaRepo.Crear(serieCarga);
            //await _serieCargaRepo.Grabar();
            // Actualizacion de cargas  
            var estudioCarga = await _estudiCargaRepo.GetBySerieCargaId(serieCarga.PACS_SerieCargaID);
            if (estudioCarga != null && estudioCarga.NumberOfSeries > 1)
                await _estudiCargaRepo.UpdateSizeForForNSerie(estudioCarga);
            //Resultado
            var serieDto = _mapper.Map<SerieDto>(serie);
            return serieDto;
        }
     
        private static SerieCarga MapSerieToCarga(Serie serie, decimal totalSizeFile)
        // Pendiente delegar a Servicio o mapeo
        {
            var serieCarga = new SerieCarga
            {
                PACS_SerieID = serie.PACS_SerieID,
                NumberOfImages = 1,
                TotalFileSizeMB = totalSizeFile,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
            return serieCarga;
        }

        public async Task<SerieDto> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            var serie = await _serieRepo.Obtener(v => v.PACS_EstudioID == id);
            if (serie == null)
                throw new ArgumentException("Serie no encontrada.");
            return _mapper.Map<SerieDto>(serie);   
        }

        public async Task<SerieDto> GetByUID(string InstanceUID)
        {
            if (!DicomUID.IsValidUid(InstanceUID))
                throw new ArgumentException("El formato de la instancia UID no es válido."); 
            var serie = await _serieRepo.Obtener(v => v.StudyInstanceUID == InstanceUID);
            if (serie == null)
                throw new KeyNotFoundException("Serie no encontrada.");
            return _mapper.Map<SerieDto>(serie);            
        }

        public async Task<IEnumerable<SerieDto>> GetAllByStudyPacsId(int studyPacsId)
        {
            if (studyPacsId <= 0)
                throw new ArgumentException("Id inválido.");
            IEnumerable<Serie> serieList = await _serieRepo.ObtenerTodos(v => v.PACS_EstudioID == studyPacsId);
            return _mapper.Map<IEnumerable<SerieDto>>(serieList);                
        }

        public async Task<IEnumerable<SerieDto>> GetAllByStudyUID(string studyUID)
        {
            //Valido formato con fo-dicom
            if (!DicomUID.IsValidUid(studyUID))
                throw new ArgumentException("El formato de la instancia UID no es válido.");
            IEnumerable<Serie> serieList = await _serieRepo.ObtenerTodos(v => v.StudyInstanceUID == studyUID);
            return _mapper.Map<IEnumerable<SerieDto>>(serieList);
        }
    }
}
