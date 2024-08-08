using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Services.IService.Pacs;
using Api_PACsServer.Utilities;
using AutoMapper;



namespace Api_PACsServer.Services
{
    public class StudyService : IStudyService
    {
        private readonly IStudyRepository _studyRepo;
        private readonly IStudyLoadRepository _studyLoadRepo;
        private readonly IMapper _mapper;

        public StudyService(IStudyRepository studyRepo, IStudyLoadRepository studyLoadRepo, IMapper mapper)
        {
            _studyRepo = studyRepo;
            _studyLoadRepo = studyLoadRepo;
            _mapper = mapper;
        }

        public async Task<Study> Create(StudyCreateDto createDto)
        {
            var studyInstanceUID = createDto.StudyInstanceUID;
            var SizeFile = createDto.TotalFileSizeMB;
            // Validate the StudyInstanceUID format
            if (!DicomUtilities.ValidateUID(studyInstanceUID))
                throw new ArgumentException("The format of the StudyInstanceUID is invalid.");
            // Check if a Study with the same UID already exists
            if (await _studyRepo.Exists(u => u.StudyInstanceUID == studyInstanceUID))
                throw new InvalidOperationException("A study with the same StudyInstanceUID already exists.");
            // Validate StudyDate
            if (createDto.StudyDate > DateTime.Today)
                throw new ArgumentException("The StudyDate cannot be in the future.");
            // Validate InstitutionID
            if (createDto.InstitutionID == 0)
                throw new ArgumentException("The InstitutionID is required.");
            // Map the DTO to the Study entity
            var study = _mapper.Map<Study>(createDto);
            await _studyRepo.Create(study);
            // Create and map StudyLoad
            var studyLoad = MapSerieLoad(study.PACSStudyID, SizeFile);
            await _studyLoadRepo.Create(studyLoad);
            // Map the Study entity to the StudyDto
            return study;
        }

        public async Task<Study> GetById(int studyId)
        {
            // Validate the ID
            if (studyId <= 0)
                throw new ArgumentException("Invalid ID.");
            // Retrieve the Study by ID
            var study = await _studyRepo.Get(v => v.PACSStudyID == studyId);
            // Check if the Study was found
            if (study == null)
                throw new KeyNotFoundException("Study not found.");
            return study;
        }

        public async Task<Study> GetByUID(string studyInstanceUID)
        {
            // Validate the StudyInstanceUID format using fo-dicom
            if (!DicomUtilities.ValidateUID(studyInstanceUID))
                throw new ArgumentException("The format of the StudyInstanceUID is invalid.");
            // Retrieve the Study by StudyInstanceUID and validate
            var study = await _studyRepo.Get(v => v.StudyInstanceUID == studyInstanceUID);
            if (study == null)
                throw new KeyNotFoundException("Study not found.");
            return study;            
        }

        public async Task<bool> ExistsByUID(string studyInstanceUID)
        {
            // Implementación de la lógica para verificar la existencia por SeriesInstanceUID
            return await _studyRepo.Exists(s => s.StudyInstanceUID == studyInstanceUID);
        }

        public async Task<StudyLoad> UpdateLoadForNewSerie(int studyId)
        {
            var studyLoad = await _studyLoadRepo.Get(u => u.PACSStudyID == studyId);
            studyLoad.UpdateDate = DateTime.UtcNow;
            studyLoad.NumberOfSeries += 1;
            return await _studyLoadRepo.Update(studyLoad);
        }

        public async Task<StudyLoad> UpdateLoadForNewInstance(int studyId, decimal totalSizeFile)
        {
            var studyLoad = await _studyLoadRepo.Get(u => u.PACSStudyID == studyId);
            studyLoad.UpdateDate = DateTime.UtcNow;
            studyLoad.TotalFileSizeMB += totalSizeFile;
            studyLoad.NumberOfFiles += 1;
            return await _studyLoadRepo.Update(studyLoad);

        }


        public UserStudiesListDto GetRecentStudies(PaginationParameters parameters)
        {
            var studiesList = _studyRepo.GetRecentStudies(parameters);
            return new UserStudiesListDto
            {
                Studies = _mapper.Map<IEnumerable<StudyDto>>(studiesList),
                TotalPages = studiesList.MetaData.TotalCount
            };
        }


        //** Private 
        private StudyLoad MapSerieLoad(int studyId, decimal totalSizeFile)
        // Pendiente delegar a Servicio o mapeo
        {
            var studyLoad = new StudyLoad
            {
                PACSStudyID = studyId,
                NumberOfFiles = 1,
                NumberOfSeries = 1,
                TotalFileSizeMB = totalSizeFile,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
            return studyLoad;
        }
    }
}
