using Api_PACsServer.Models;
using Api_PACsServer.Models.DicomSupport;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Models.OHIFVisor;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Repositories.IRepository.DicomSupport;
using Api_PACsServer.Repositories.IRepository.MainEntities;
using Api_PACsServer.Repositories.IRepository.Supplement;
using Api_PACsServer.Services.IService;
using Api_PACsServer.Services.IService.MainEntities;
using AutoMapper;
using FellowOakDicom;

namespace Api_PACsServer.Services
{
    public class StudyService : IStudyService
    {
        private readonly IStudyRepository _studyRepo;
        private readonly IStudyDetailsRepository _studyDetailsRepo;
        private readonly IStudyModalityRepository _studyModalityRepo;
        private readonly IQueryBuilderService _queryBuilderService;
        private readonly IMapper _mapper;


        public StudyService(IStudyRepository studyRepo, IStudyDetailsRepository studyDetailsRepo,
                            IStudyModalityRepository studyModalityRepo, IQueryBuilderService queryBuilderService, 
                            IMapper mapper)
        {
            _studyRepo = studyRepo;
            _studyDetailsRepo = studyDetailsRepo;
            _studyModalityRepo = studyModalityRepo;
            _queryBuilderService = queryBuilderService;
            _mapper = mapper;
        }

        public async Task<Study> Create(StudyCreateDto CreateDto)
        {
            var studyInstanceUID = CreateDto.StudyInstanceUID;
            var SizeFile = CreateDto.TotalFileSizeMB;
            // Check if a Study with the same UID already exists
            if (await _studyRepo.Exists(u => u.StudyInstanceUID == studyInstanceUID))
                throw new InvalidOperationException("A study with the same StudyInstanceUID already exists.");
            // Validate StudyDate
            if (CreateDto.StudyDate > DateTime.Today)
                throw new ArgumentException("StudyDate cannot be in the future.");
            // Map the DTO to the Study entity
            var study = _mapper.Map<Study>(CreateDto);
            study.CreationDate = DateTime.UtcNow;
            await _studyRepo.Create(study);
            // Create and map Study Details
            var studyDetailsCreate = new StudyDetailsCreateDto(study.StudyInstanceUID, SizeFile);
            var studyDetails = _mapper.Map<StudyDetails>(studyDetailsCreate);
            await _studyDetailsRepo.Create(studyDetails);
            // Register modality
            var studyModalityCreate = _mapper.Map<StudyModalityCreateDto>(CreateDto);
            var studyModality = _mapper.Map<StudyModality>(studyModalityCreate);
            studyModality.CreationDate = DateTime.UtcNow;
            await _studyModalityRepo.Create(studyModality);
            // Map the Study entity to the StudyDto
            return study;
        }

        public async Task<Study> GetById(int studyId)
        {
            // Validate the ID
            if (studyId <= 0)
                throw new ArgumentException("Invalid ID.");
            // Retrieve the Study by ID
            var study = await _studyRepo.Get(v => v.StudyID == studyId);
            // Check if the Study was found
            if (study == null)
                throw new KeyNotFoundException("Study not found.");
            return study;
        }

        public async Task<Study> GetByUID(string studyInstanceUID)
        {
            // Retrieve the Study by StudyInstanceUID and validate
            if (!DicomUID.IsValidUid(studyInstanceUID))
                throw new KeyNotFoundException("StudyInstanceUID not valid.");
            var study = await _studyRepo.Get(v => v.StudyInstanceUID == studyInstanceUID);
            if (study == null)
                throw new KeyNotFoundException("Study not found.");
            return study;            
        } 
        
        public async Task<OHIFStudy> GetOHIFByUID(string studyInstanceUID)
        {
            // Retrieve the Study by StudyInstanceUID and validate
            var study = await _studyRepo.Get(v => v.StudyInstanceUID == studyInstanceUID);
            if (study == null)
                throw new KeyNotFoundException("Study not found.");
            // Utiliza AutoMapper para convertir la entidad 'Instance' a 'OHIFInstance'
            return _mapper.Map<OHIFStudy>(study);           
        }

        public async Task<bool> ExistsByUID(string studyInstanceUID)
        {
            // Implementación de la lógica para verificar la existencia por SeriesInstanceUID
            if (!DicomUID.IsValidUid(studyInstanceUID))
                throw new KeyNotFoundException("StudyInstanceUID not valid.");
            return await _studyRepo.Exists(s => s.StudyInstanceUID == studyInstanceUID);
        }

        public async Task UpdateForNewSerie(string studyInstanceUID, string modality)
        {
            var study = await _studyRepo.Get(s => s.StudyInstanceUID == studyInstanceUID);
            // update Details
            await UpdateDetailsForNewSerie(studyInstanceUID);
            // Verificar si ya existe la modalidad para el estudio
            bool modalityExists = await _studyModalityRepo.Exists(sm => sm.StudyInstanceUID == studyInstanceUID && sm.Modality == modality);
            if (!modalityExists)
            {
                // Registrar la modalidad solo si no existe
                var studyModality = new StudyModality
                {
                    StudyInstanceUID = studyInstanceUID,
                    Modality = modality,
                    CreationDate = DateTime.Now
                };
                await _studyModalityRepo.Create(studyModality);
            }
        }

        internal async Task<StudyDetails> UpdateDetailsForNewSerie(string studyInstanceUID)
        {
            var studyDetails = await _studyDetailsRepo.Get(u => u.StudyInstanceUID == studyInstanceUID);
            var studyDetailsUpdateDto = _mapper.Map<StudyDetailsUpdateDto>(studyDetails);
            studyDetailsUpdateDto.NumberOfStudyRelatedSeries ++;
            _mapper.Map(studyDetailsUpdateDto, studyDetails);
            return await _studyDetailsRepo.Update(studyDetails);
        }

        public async Task<StudyDetails> UpdateDetailsForNewInstance(string studyInstanceUID, decimal totalSizeFile)
        {
            // Get study details and configure updateDto
            var studyDetails = await _studyDetailsRepo.Get(u => u.StudyInstanceUID == studyInstanceUID);
            var studyDetailsUpdateDto = _mapper.Map<StudyDetailsUpdateDto>(studyDetails);
            studyDetailsUpdateDto.TotalFileSizeMB = studyDetailsUpdateDto.TotalFileSizeMB + totalSizeFile;
            studyDetailsUpdateDto.NumberOfStudyRelatedInstances ++;
            // mapping to entity and update
            _mapper.Map(studyDetailsUpdateDto, studyDetails);
            return await _studyDetailsRepo.Update(studyDetails);
        }

        //public UserStudiesListDto GetRecentStudies(PaginationParameters parameters)
        //{
        //    var studiesList = _studyRepo.GetRecentStudies(parameters);
        //    // Usar AutoMapper para mapear la lista de Study a StudyDto, incluyendo los campos adicionales
        //    var studiesDtoList = _mapper.Map<IEnumerable<StudyDto>>(studiesList);
        //    return new UserStudiesListDto
        //    {
        //        Studies = studiesDtoList,
        //        TotalPages = studiesList.MetaData.TotalCount
        //    };
        //}

        //public async Task<StudyCreateDto> MapToCreateDto(MainEntitiesCreateDto metadata)
        //{
        //    return _mapper.Map<StudyCreateDto>(metadata);
        //}

        public async Task<StudyCreateDto> MapMetadataToCreateDto(MetadataDto metadata)
        {
            return _mapper.Map<StudyCreateDto>(metadata);
        }

        public async Task<List<StudyDto>> GetStudyData(QueryRequestParameters<StudyQueryParametersDto> requestParameters) 
        {

            // Validate DTOs before proceeding.
            //ValidateStudyParameters(requestParameters.DicomParameters);
            var querySpecifications = _queryBuilderService.BuildQuerySpecification(requestParameters);

            // Call repository to execute query
            var studies = await _studyRepo.ExecuteStudyQuery(querySpecifications);
            // 2. Obtener StudyDetails asociados a los StudyInstanceUID de los estudios
            var studyInstanceUIDs = studies.Select(s => s.StudyInstanceUID).ToList();

            var studyDetails = await _studyDetailsRepo.GetDetailsByUIDs(studyInstanceUIDs);

            // Map results to StudyDto
            var studiesDto = new List<StudyDto>();
            for (int i = 0; i < studies.Count; i++)
            {
                var studyDto = _mapper.Map<(Study, StudyDetails), StudyDto>((studies[i], studyDetails[i]));
                studiesDto.Add(studyDto);
            }

            return studiesDto;
        } 
    }
}
