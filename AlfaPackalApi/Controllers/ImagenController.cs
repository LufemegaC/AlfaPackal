using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto;
using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
            _response = new();
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

        [HttpGet("{id:int}", Name = "GetImagen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetImagen(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError("Error al traer imagen con Id " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var imagen = await _imagenRepo.ObtenerPorID(v => v.ImagenID == id);
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

        [HttpGet("{instanceUID}", Name = "GetImageByIdInstanceUID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetImageByIdInstanceUID(string instanceUID)
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
                var imagen = await _imagenRepo.GetImageByInstanceUID(instanceUID);
                if (imagen == null) // Si el registro no fue encontrado
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

        [HttpPost]
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
                if (await _imagenRepo.ExisteImagenInstanceUID(CreateDto.SOPInstanceUID))
                {
                    return BadRequest("Ya existe un estudio con el mismo SOPInstanceUID.");
                }
                Imagen modelo = _mapper.Map<Imagen>(CreateDto);
                await _imagenRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetImagen", new { ImagenID = modelo.ImagenID }, _response);
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
                var imagen = await _imagenRepo.ObtenerPorID(v => v.ImagenID == id);
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

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateImagen(int id, [FromBody] ImagenUpdateDto UpdateDto)
        {
            try
            {
                if (UpdateDto == null || id != UpdateDto.ImagenID)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var imagenExistente = await _imagenRepo.ObtenerPorID(x => x.ImagenID == id);
                if (imagenExistente == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                Imagen modelo = _mapper.Map<Imagen>(UpdateDto);
                await _imagenRepo.Actualizar(modelo);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialImagen(int id, JsonPatchDocument<ImagenUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            var imagenExistente = await _imagenRepo.ObtenerPorID(x => x.ImagenID == id, tracked: false);
            if (imagenExistente == null)
            {
                return NotFound("Imagen no encontrada.");
            }
            ImagenUpdateDto imagenDto = _mapper.Map<ImagenUpdateDto>(imagenExistente);
            patchDto.ApplyTo(imagenDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Imagen modelo = _mapper.Map<Imagen>(imagenDto);
            await _imagenRepo.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}