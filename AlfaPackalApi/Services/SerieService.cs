using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Models.Dto.Series;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Models.OHIFVisor;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Repositories.IRepository.MainEntities;
using Api_PACsServer.Repositories.IRepository.Supplement;
using Api_PACsServer.Services.IService;
using Api_PACsServer.Services.IService.MainEntities;
using AutoMapper;

namespace Api_PACsServer.Services
{
    public class SerieService : ISerieService
    {
        private readonly ISerieRepository _serieRepo;
        private readonly ISerieDetailsRepository _serieDetailsRepo;
        private readonly IQueryBuilderService _queryBuilderService;
        private readonly IMapper _mapper;

        public SerieService(ISerieRepository serieRepo, ISerieDetailsRepository serieDetailsRepo,
                            IQueryBuilderService queryBuilderService, IMapper mapper)
        {
            _serieRepo = serieRepo;
            _serieDetailsRepo = serieDetailsRepo;
            _queryBuilderService = queryBuilderService;
            _mapper = mapper;
        }

        public async Task<Serie> Create(SerieCreateDto createDto)
        {
            
            var serieInstanceUID = createDto.SeriesInstanceUID;
            //if (!DicomUtilities.ValidateUID(serieInstanceUID))
            //    throw new ArgumentException("The format of the instance UID is not valid.");
            // Specific validation for StudyInstanceUID
            if (await _serieRepo.Exists(u => u.SeriesInstanceUID == serieInstanceUID))
                throw new InvalidOperationException("The instance UID already exists.");
            // Series entity registration
            var serie = _mapper.Map<Serie>(createDto);
            serie.CreationDate = DateTime.UtcNow;
            await _serieRepo.Create(serie);
            // Details information registration
            var serieDetailsCreate = new SerieDetailsCreateDto(serie.SeriesInstanceUID, createDto.TotalFileSizeMB);
            var serieDetails = _mapper.Map<SerieDetails>(serieDetailsCreate);
            await _serieDetailsRepo.Create(serieDetails);
            // return dto
            return serie;
        }
     
        //public async Task<Serie> GetById(int studyId, int seriesNumber)
        //{
        //    if (studyId <= 0 || seriesNumber <= 0)
        //        throw new ArgumentException("Invalid IDs value.");
        //    var serie = await _serieRepo.Get(v => v.StudyID == studyId &&
        //                                     v.SeriesNumber == seriesNumber);
        //    if (serie == null)
        //        throw new ArgumentException("Series not found.");
        //    return serie;   
        //}

        public async Task<Serie> GetByUID(string serieInstacenUID)
        {
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

        public async Task<IEnumerable<OHIFSerie>> GetAllByStudyUID(string studyInstanceUID)
        {
            if (string.IsNullOrEmpty(studyInstanceUID))
                throw new ArgumentException("Invalid study UID value.");
            IEnumerable<Serie> serieList = await _serieRepo.GetAll(v => v.StudyInstanceUID == studyInstanceUID);
            return _mapper.Map<IEnumerable<OHIFSerie>>(serieList);
        }

        public async Task<SerieDetails> UpdateDetailsForNewInstance(string seriesInstanceUID, decimal totalSizeFile)
        {
            var serieDetails = await _serieDetailsRepo.Get(u => u.SeriesInstanceUID == seriesInstanceUID);
            var serieDetailsUpdateDto = _mapper.Map<SerieDetailsUpdateDto>(serieDetails);

            serieDetailsUpdateDto.TotalFileSizeMB = serieDetailsUpdateDto.TotalFileSizeMB + totalSizeFile;
            serieDetailsUpdateDto.NumberOfSeriesRelatedInstances++;
            _mapper.Map(serieDetailsUpdateDto, serieDetails);
            return await _serieDetailsRepo.Update(serieDetails);
        }

        //public async Task<SerieCreateDto> MapToCreateDto(MainEntitiesCreateDto metadata)
        //{
        //    return _mapper.Map<SerieCreateDto>(metadata);
        //}

        public async Task<SerieCreateDto> MapMetadataToCreateDto(MetadataDto metadata)
        {
            return _mapper.Map<SerieCreateDto>(metadata);
        }

        public async Task<List<SerieDto>> GetSeriesFromStudy(QueryRequestParameters<SerieQueryParametersDto> requestParameters)
        {
            // Validate DTOs before proceeding.
            //ValidateStudyParameters(requestParameters.DicomParameters);
            var querySpecifications = _queryBuilderService.BuildQuerySpecification(requestParameters);
            var series = await _serieRepo.ExecuteSerieQuery(querySpecifications);
            var serieInstanceUIDs = series.Select(s => s.SeriesInstanceUID).ToList();
            // Call repository to execute query
            var seriesDetails = await _serieDetailsRepo.GetDetailsByUIDs(serieInstanceUIDs);
            
            // Map results to StudyDto
            var seriesDto = new List<SerieDto>();
            for (int i = 0; i < series.Count; i++)
            {
                var serieDto = _mapper.Map<(Serie, SerieDetails), SerieDto>((series[i], seriesDetails[i]));
                seriesDto.Add(serieDto);
            }

            return seriesDto;
        }
    }
}
