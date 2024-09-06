using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.Instances;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Services.IService.Pacs;
using AutoMapper;


namespace Api_PACsServer.Services
{
    public class InstanceService : IInstanceService
    {
        private readonly IInstanceRepository _instanceRepo;
        private readonly IInstanceDetailsRepository _instanceLoadRepo;
        private readonly IMapper _mapper;

        public InstanceService(IInstanceRepository instanceRepo, IInstanceDetailsRepository instanceLoadRepo,
                             IMapper mapper)
        {
            _instanceRepo = instanceRepo;
            _instanceLoadRepo = instanceLoadRepo;
            _mapper = mapper;
        }

        public async Task<Instance> Create(InstanceCreateDto createDto)
        {
            var InstanceUID = createDto.SOPInstanceUID;
            var SizeFile = createDto.TotalFileSizeMB;
            //if (!DicomUtilities.ValidateUID(InstanceUID))
            //    throw new ArgumentException("The instance UID format is not valid.");
            // Validate if UID already exists
            if (await _instanceRepo.Exists(u => u.SOPInstanceUID == InstanceUID))
                throw new InvalidOperationException("An image with the same SOPInstanceUID already exists.");
            // register main entity
            var instance = _mapper.Map<Instance>(createDto);
            instance.CreationDate = DateTime.UtcNow;
            await _instanceRepo.Create(instance);
            // register load entity
            var instanceDetailsCreate = new InstanceDetailsCreateDto(instance.SOPInstanceUID, SizeFile);
            var instanceDetails = _mapper.Map<InstanceDetails>(instanceDetailsCreate);
            await _instanceLoadRepo.Create(instanceDetails);
            // response
            return instance;
        }

        //public async Task<Instance> GetByIdComponents(int studyId, int seriesNumber, int instanceNumber)
        //{
        //    if (studyId <= 0 || seriesNumber <= 0 || instanceNumber <= 0)
        //        throw new ArgumentException("Invalid Id value.");
        //    // Retrieve the instance associated with the specified ID
        //    var instance = await _instanceRepo.Get(v => v.StudyID == studyId && v.SeriesNumber == seriesNumber
        //                                            &&  v.InstanceNumber == instanceNumber);
        //    // Check if the instance was found
        //    if (instance == null)
        //        throw new KeyNotFoundException("Instance not found.");
        //    return instance;
        //}

        public async Task<Instance> GetByUID(string SOPInstanceUID)
        {
            // Retrieve the instance associated with the specified UID
            var instance = await _instanceRepo.Get(v => v.SOPInstanceUID == SOPInstanceUID);
            if (instance == null)
                throw new KeyNotFoundException("Instance not found.");
            return instance;
        }

        public async Task<bool> ExistsByUID(string serieInstanceUID)
        {
            // Implementación de la lógica para verificar la existencia por SeriesInstanceUID
            return await _instanceRepo.Exists(s => s.SOPInstanceUID == serieInstanceUID);
        }

        public async Task<IEnumerable<InstanceDto>> GetAllBySerieUID(string seriesIntanceUID)
        {
            // Validate the series ID
            if (string.IsNullOrEmpty(seriesIntanceUID))
                throw new ArgumentException("Invalid seriess UID .");
            // Retrieve all instances associated with the specified series ID
            IEnumerable<Instance> instanceList = await _instanceRepo.GetAll(v => v.SeriesInstanceUID == seriesIntanceUID);
            // Check if any instances were found
            if (instanceList == null)
                throw new KeyNotFoundException("Instances not found.");
            // Map the instances to the corresponding DTOs and return them
            return _mapper.Map<IEnumerable<InstanceDto>>(instanceList);
        }

        public async Task<InstanceCreateDto> MapToCreateDto(MainEntitiesCreateDto metadata)
        {
            return _mapper.Map<InstanceCreateDto>(metadata);
        }
    }
}
