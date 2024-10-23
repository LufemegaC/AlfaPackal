
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Models.OHIFVisor;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Services.IService.Pacs;
using AutoMapper;


namespace Api_PACsServer.Services
{
    public class StudyService : IStudyService
    {
        private readonly IStudyRepository _studyRepo;
        private readonly IStudyDetailsRepository _studyDetailsRepo;
        private readonly IMapper _mapper;

        public StudyService(IStudyRepository studyRepo, IStudyDetailsRepository studyLoadRepo, IMapper mapper)
        {
            _studyRepo = studyRepo;
            _studyDetailsRepo = studyLoadRepo;
            _mapper = mapper;
        }

        public async Task<Study> Create(StudyCreateDto CreateDto)
        {
            var studyInstanceUID = CreateDto.StudyInstanceUID;
            var SizeFile = CreateDto.TotalFileSizeMB;
            //// Validate the StudyInstanceUID format
            //if (!DicomUtilities.ValidateUID(studyInstanceUID))
            //    throw new ArgumentException("The format of the StudyInstanceUID is invalid.");
            // Check if a Study with the same UID already exists
            if (await _studyRepo.Exists(u => u.StudyInstanceUID == studyInstanceUID))
                throw new InvalidOperationException("A study with the same StudyInstanceUID already exists.");
            // Validate StudyDate
            if (CreateDto.StudyDate > DateTime.Today)
                throw new ArgumentException("StudyDate cannot be in the future.");
            // Validate InstitutionID
            ////if (CreateDto.InstitutionID == 0)
            ////    throw new ArgumentException("The Institution ID is required.");
            // Map the DTO to the Study entity
            var study = _mapper.Map<Study>(CreateDto);
            study.CreationDate = DateTime.UtcNow;
            await _studyRepo.Create(study);
            // Create and map StudyLoad
            var studyDetailsCreate = new StudyDetailsCreateDto(study.StudyInstanceUID, SizeFile);
            var studyDetails = _mapper.Map<StudyDetails>(studyDetailsCreate);
            await _studyDetailsRepo.Create(studyDetails);
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
            return await _studyRepo.Exists(s => s.StudyInstanceUID == studyInstanceUID);
        }

        public async Task<StudyDetails> UpdateDetailsForNewSerie(string studyInstanceUID)
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

        public UserStudiesListDto GetRecentStudies(PaginationParameters parameters)
        {
            var studiesList = _studyRepo.GetRecentStudies(parameters);
            // Usar AutoMapper para mapear la lista de Study a StudyDto, incluyendo los campos adicionales
            var studiesDtoList = _mapper.Map<IEnumerable<StudyDto>>(studiesList);
            return new UserStudiesListDto
            {
                Studies = studiesDtoList,
                TotalPages = studiesList.MetaData.TotalCount
            };
        }

        //public async Task<StudyCreateDto> MapToCreateDto(MainEntitiesCreateDto metadata)
        //{
        //    return _mapper.Map<StudyCreateDto>(metadata);
        //}

        public async Task<StudyCreateDto> MapMetadataToCreateDto(MetadataDto metadata)
        {
            return _mapper.Map<StudyCreateDto>(metadata);
        }
    }
}
