using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Models.Dto.Series;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Services.IService.Pacs;
using Api_PACsServer.Utilities;
using AutoMapper;



namespace Api_PACsServer.Services
{
    public class SerieService : ISerieService
    {
        private readonly ISerieRepository _serieRepo;
        private readonly ISerieLoadRepository _serieLoadRepo;
        private readonly IMapper _mapper;

        public SerieService(ISerieRepository serieRepo, ISerieLoadRepository serieLoadRepo,
                            IMapper mapper)
        {
            _serieRepo = serieRepo;
            _serieLoadRepo = serieLoadRepo;
            _mapper = mapper;
        }

        public async Task<Serie> Create(SerieCreateDto createDto)
        {
            var serieInstanceUID = createDto.SeriesInstanceUID;
            if (!DicomUtilities.ValidateUID(serieInstanceUID))
                throw new ArgumentException("The format of the instance UID is not valid.");
            // Specific validation for StudyInstanceUID
            if (await _serieRepo.Exists(u => u.SeriesInstanceUID == serieInstanceUID))
                throw new InvalidOperationException("The instance UID already exists.");
            // Series entity registration
            var serie = _mapper.Map<Serie>(createDto);
            serie.CreationDate = DateTime.UtcNow;
            await _serieRepo.Create(serie);
            // Load information registration
            var serieLoad = MapSerieToCarga(serie.PACSSerieID, createDto.TotalFileSizeMB);
            await _serieLoadRepo.Create(serieLoad);
            // return dto
            return serie;
        }
     
        public async Task<Serie> GetById(int serieId)
        {
            if (serieId <= 0)
                throw new ArgumentException("Invalid ID.");
            var serie = await _serieRepo.Get(v => v.PACSStudyID == serieId);
            if (serie == null)
                throw new ArgumentException("Series not found.");
            return serie;   
        }

        public async Task<Serie> GetByUID(string serieInstacenUID)
        {
            if (!DicomUtilities.ValidateUID(serieInstacenUID))
                throw new ArgumentException("The instance UID format is not valid.");
            var serie = await _serieRepo.Get(v => v.SeriesInstanceUID == serieInstacenUID);
            if (serie == null)
                throw new KeyNotFoundException("Series not found.");
            return serie;            
        }

        public async Task<bool> ExistsByUID(string serieInstacenUID)
        {
            // Implementación de la lógica para verificar la existencia por SeriesInstanceUID
            return await _serieRepo.Exists(s => s.SeriesInstanceUID == serieInstacenUID);
        }

        public async Task<IEnumerable<SerieDto>> GetAllByStudyPacsId(int studyId)
        {
            if (studyId <= 0)
                throw new ArgumentException("Invalid ID.");
            IEnumerable<Serie> serieList = await _serieRepo.GetAll(v => v.PACSStudyID == studyId);
            return _mapper.Map<IEnumerable<SerieDto>>(serieList);                
        }

        //public async Task<IEnumerable<SerieDto>> GetAllByStudyUID(string studyInstanceUID)
        //{
        //    var study = await
            
            
            
        //    // Validate format with fo-dicom
        //    if (!DicomUtilities.ValidateUID(studyInstanceUID))
        //        throw new ArgumentException("The instance UID format is not valid.");
            
            
        //    IEnumerable<Serie> serieList = await _serieRepo.GetAll(u => u.InstastudyInstanceUID);

        //    return _mapper.Map<IEnumerable<SerieDto>>(serieList);
        //}

        public async Task<SerieLoad> UpdateLoadForNewInstance(int serieId, decimal totalSizeFile)
        {
            var serieLoad = await _serieLoadRepo.Get(u => u.PACSSerieID == serieId);
            serieLoad.UpdateDate = DateTime.UtcNow;
            serieLoad.TotalFileSizeMB += totalSizeFile;
            serieLoad.NumberOfInstances += 1;
            return await _serieLoadRepo.Update(serieLoad);
        }

        //** Private
        private SerieLoad MapSerieToCarga(int SerieID, decimal totalSizeFile)
        // Pendiente delegar a Servicio o mapeo
        {
            var serieCarga = new SerieLoad
            {
                PACSSerieID = SerieID,
                NumberOfInstances = 1,
                TotalFileSizeMB = totalSizeFile,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
            return serieCarga;
        }
    }
}
