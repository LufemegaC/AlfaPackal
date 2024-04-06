using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto;
using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Api_PACsServer.Modelos.Dto;
using static Utileria.DicomUtilities;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenController : ControllerBase
    {
        private readonly ILogger<ImagenController> _logger;
        private readonly IImagenRepositorio _imagenRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public ImagenController(ILogger<ImagenController> logger, IImagenRepositorio imagenRepo, IMapper mapper)
        {
            _logger = logger;
            _imagenRepo = imagenRepo;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetImagenes()
        {
            try
            {
                _logger.LogInformation("Obtener las Imagenes");
                IEnumerable<Imagen> imagenList = await _imagenRepo.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<ImagenDto>>(imagenList);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetImagenByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetImagenByID(int id)
        {
            try
            {
                if (id <= 0)
                {
                    //_logger.LogError("Error al traer imagen con Id " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var imagen = await _imagenRepo.ObtenerPorID(v => v.PACS_ImagenID == id);
                if (imagen == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<ImagenDto>(imagen);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_mapper.Map<ImagenDto>(imagen));
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("GetImageByInstanceUID/{instanceUID}", Name = "GetImageByInstanceUID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetImageByInstanceUID(string instanceUID)
        {
            try
            {
                //Si el formate de la instancia UID no es valido
                if (!IsValidDicomUid(instanceUID))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                // Localizo registro en el modelo
                var imagen = await _imagenRepo.GetBySOPInstanceUID(instanceUID);
                if (imagen == null) // Si el registro no fue encontrado
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<ImagenDto>(imagen);
                _response.StatusCode = HttpStatusCode.OK;
                _response.PacsResourceId = imagen.PACS_ImagenID;
                return Ok(_mapper.Map<ImagenDto>(imagen));
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("ExistBySOPInstanceUID/{instanceUID}", Name = "ExistBySOPInstanceUID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> ExistBySOPInstanceUID(string instanceUID)
        {
            try
            {
                //Si el formate de la instancia UID no es valido
                if (!IsValidDicomUid(instanceUID))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var exists = await _imagenRepo.ExistBySOPInstanceUID(instanceUID);
                _response.Resultado = exists;
                _response.StatusCode = exists ? HttpStatusCode.OK : HttpStatusCode.NotFound;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("CrearImagen")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearImagen([FromBody] ImagenCreateDto CreateDto)
        {
            try
            {
                if (!ModelState.IsValid || CreateDto == null)
                {
                    return BadRequest(ModelState);
                }
                if(!IsValidDicomUid(CreateDto.SOPInstanceUID))
                {
                    return BadRequest(ModelState);
                }
                // Validación específica para SOPInstanceUID
                if (await _imagenRepo.ExistBySOPInstanceUID(CreateDto.SOPInstanceUID))
                {
                    return BadRequest("Ya existe un estudio con el mismo SOPInstanceUID.");
                }
                Imagen modelo = _mapper.Map<Imagen>(CreateDto);
                await _imagenRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;
                _response.PacsResourceId = modelo.PACS_ImagenID;
                return CreatedAtRoute("GetImagenByID", new { id = modelo.PACS_ImagenID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImagen(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var imagen = await _imagenRepo.ObtenerPorID(v => v.PACS_ImagenID == id);
                if (imagen == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _imagenRepo.Remover(imagen);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }
    }
}