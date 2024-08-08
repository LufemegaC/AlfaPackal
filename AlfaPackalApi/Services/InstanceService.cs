using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Models.Dto.Instances;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Services.IService.Pacs;
using Api_PACsServer.Utilities;
using AutoMapper;


namespace Api_PACsServer.Services
{
    public class InstanceService : IInstanceService
    {
        private readonly IInstanceRepository _instanceRepo;
        private readonly IInstanceLoadRepository _instanceLoadRepo;
        private readonly IMapper _mapper;

        public InstanceService(IInstanceRepository instanceRepo, IInstanceLoadRepository instanceLoadRepo,
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
            if (!DicomUtilities.ValidateUID(InstanceUID))
                throw new ArgumentException("The instance UID format is not valid.");
            // Validate if UID already exists
            if (await _instanceRepo.Exists(u => u.SOPInstanceUID == InstanceUID))
                throw new InvalidOperationException("An image with the same SOPInstanceUID already exists.");
            // register main entity
            Instance mainModel = _mapper.Map<Instance>(createDto);
            mainModel.CreationDate = DateTime.UtcNow;
            await _instanceRepo.Create(mainModel);
            // register load entity
            InstanceLoad instanceLoad = MapImagenToLoad(mainModel.PACSInstanceID, SizeFile);
            await _instanceLoadRepo.Create(instanceLoad);
            // response
            return mainModel;
        }

        public async Task<Instance> GetById(int instanceId)
        {
            if (instanceId <= 0)
                throw new ArgumentException("Invalid ID.");
            // Retrieve the instance associated with the specified ID
            var instance = await _instanceRepo.Get(v => v.PACSInstanceID == instanceId);
            // Check if the instance was found
            if (instance == null)
                throw new KeyNotFoundException("Instance not found.");
            return instance;
        }

        public async Task<Instance> GetByUID(string SOPInstanceUID)
        {
            // Validate the format of the instance UID using fo-dicom
            if (!DicomUtilities.ValidateUID(SOPInstanceUID))
                throw new ArgumentException("The instance UID format is not valid.");
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

        public async Task<IEnumerable<InstanceDto>> GetAllBySerieId(int serieId)
        {
            // Validate the series ID
            if (serieId <= 0)
                throw new ArgumentException("Invalid ID.");
            // Retrieve all instances associated with the specified series ID
            IEnumerable<Instance> instanceList = await _instanceRepo.GetAll(v => v.PACSSerieID == serieId);
            // Check if any instances were found
            if (instanceList == null)
                throw new KeyNotFoundException("Instances not found.");
            // Map the instances to the corresponding DTOs and return them
            return _mapper.Map<IEnumerable<InstanceDto>>(instanceList);
        }

        // MIRGRAR A ORCHESTRATOR
        //public async Task<IEnumerable<InstanceDto>> GetAllBySerieUID(string serieInstanceUID)
        //{
        //    // Validate UID
        //    if (!DicomUtilities.ValidateUID(serieInstanceUID))
        //        throw new ArgumentException("The instance UID format is not valid.");
        //    // Retrieve all instances associated with the specified series UID
        //    IEnumerable<Instance> instanceList = await _instanceRepo.GetAll(u => u.sserieInstanceUID);
        //    // Check if any instances were found
        //    if (instanceList == null)
        //        throw new KeyNotFoundException("Instances not found.");
        //    return _mapper.Map<IEnumerable<InstanceDto>>(instanceList);
        //}
        // ** Private
        private static InstanceLoad MapImagenToLoad(int instanceID, decimal totalSizeFile)
        // Pendiente delegar a Servicio o mapeo
        {
            var instanceload = new InstanceLoad
            {
                PACSInstanceID = instanceID,
                TotalFileSizeMB = totalSizeFile,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };
            return instanceload;
        }
    }
}
