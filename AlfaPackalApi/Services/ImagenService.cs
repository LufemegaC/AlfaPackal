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
    public class ImagenService : IImagenService
    {
        private readonly IImagenRepositorio _imagenRepo;
        private readonly IImagenCargaRepositorio _imagenCargaRepo;
        private readonly IEstudioCargaRepositorio _estudioCargaRepo;
        private readonly ISerieCargaRepositorio _serieCargaRepo;
        private readonly IMapper _mapper;


        public ImagenService(IImagenRepositorio imagenRepo, IImagenCargaRepositorio imagenCargaRepo,
                             IEstudioCargaRepositorio estudioCargaRepo, ISerieCargaRepositorio serieCargaRepo,IMapper mapper)
        {
            _imagenRepo = imagenRepo;
            _imagenCargaRepo = imagenCargaRepo;
            _estudioCargaRepo = estudioCargaRepo;
            _serieCargaRepo = serieCargaRepo;
            _mapper = mapper;
        }

        public async Task<ImagenDto> Create(ImagenCreateDto createDto)
        {

            var InstanceUID = createDto.SOPInstanceUID;
            var SizeFile = createDto.TotalFileSizeMB;
            if (!DicomUID.IsValidUid(InstanceUID))
                throw new ArgumentException("El formato de la instancia UID no es válido.");
            // Validación específica para SOPInstanceUID
            if (await _imagenRepo.ExistBySOPInstanceUID(InstanceUID))
                throw new InvalidOperationException("Ya existe una imagen con el mismo SOPInstanceUID.");
            // Registra entidad principal
            Imagen modelo = _mapper.Map<Imagen>(createDto);
            modelo.CreationDate = DateTime.UtcNow;
            await _imagenRepo.Crear(modelo);
            // Registra entidad de carga
            ImagenCarga modeloCarga = MapImagenToCarga(modelo, SizeFile);
            await _imagenCargaRepo.Crear(modeloCarga);
            // Actualizacion de informacion de carga
            EstudioCarga estudioCarga = await _estudioCargaRepo.GetByImageCargaId(modeloCarga.PACS_ImagenCargaID);
            if (estudioCarga != null && estudioCarga.NumberOfFiles != 1)
                await _estudioCargaRepo.UpdateSizeForNImage(estudioCarga, SizeFile);
            SerieCarga serieCarga = await _serieCargaRepo.GetByImagenCargaId(modeloCarga.PACS_ImagenCargaID);
            if (serieCarga != null && serieCarga.NumberOfImages != 1)
                await _serieCargaRepo.UpdateSizeForNImage(serieCarga, SizeFile);
            // respuesta
            var imagenDto = _mapper.Map<ImagenDto>(modelo);
            imagenDto.TotalFileSizeMB = SizeFile;
            //Resultado
            return imagenDto;
        }
        
        public async Task<IEnumerable<ImagenDto>> GetAllBySerieId(int pacsSerieId)
        {
            if (pacsSerieId <= 0)
                throw new ArgumentException("Id inválido.");
            IEnumerable<Imagen> imagenList = await _imagenRepo.ObtenerTodos(v => v.PACS_SerieID == pacsSerieId);
            return _mapper.Map<IEnumerable<ImagenDto>>(imagenList);
        }

        public async Task<IEnumerable<ImagenDto>> GetAllBySerieUID(string serieUID)
        {
            if (!DicomUID.IsValidUid(serieUID))
                throw new ArgumentException("El formato de la instancia UID no es válido.");
            IEnumerable<Imagen> imagenList = await _imagenRepo.ObtenerTodos(v => v.SeriesInstanceUID == serieUID);
            if (imagenList == null)
                throw new KeyNotFoundException("Imagen no encontrada.");
            return _mapper.Map<IEnumerable<ImagenDto>>(imagenList);
        }

        private static ImagenCarga MapImagenToCarga(Imagen imagen, decimal totalSizeFile)
        // Pendiente delegar a Servicio o mapeo
        {
            var imagenCarga = new ImagenCarga
            {
                PACS_ImagenID = imagen.PACS_ImagenID,
                NumberOfFrames = imagen.NumberOfFrames,
                TotalFileSizeMB = totalSizeFile,
                CreationDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };
            return imagenCarga;
        }

        public async Task<ImagenDto> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");
            var imagen = await _imagenRepo.Obtener(v => v.PACS_ImagenID == id);
            if (imagen == null)
                throw new KeyNotFoundException("Imagen no encontrada.");
            return _mapper.Map<ImagenDto>(imagen);
        }

        public async Task<ImagenDto> GetByUID(string InstanceUID)
        {
            //Valido formato con fo-dicom
            if (!DicomUID.IsValidUid(InstanceUID))
                throw new ArgumentException("El formato de la instancia UID no es válido.");
            var imagen = await _imagenRepo.Obtener(v => v.SOPInstanceUID == InstanceUID);
            if(imagen == null)
                throw new KeyNotFoundException("Imagen no encontrado.");
            return _mapper.Map<ImagenDto>(imagen);
        }
    }
}
