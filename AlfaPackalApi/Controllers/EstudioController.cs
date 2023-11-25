using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto;
using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudioController : ControllerBase
    {
        private readonly ILogger<EstudioController> _logger;
        // private readonly ApplicationDbContext _db; Antes de implementar "Patron de Repositorio" 
        private readonly IEstudioRespositorio _estudioRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public EstudioController(ILogger<EstudioController> logger, IEstudioRespositorio estudioRepo, IMapper mapper)
        {
            _logger = logger;
            _estudioRepo = estudioRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetEstudios()
        /*Metodo para obtener todas las Estudios*/
        {
            try
            {
                _logger.LogInformation("Obtener las Estudios");
                //IEnumerable<Estudio> EstudioList = await _db.Estudios.ToListAsync(); //Consulta las Estudios en listado
                IEnumerable<Estudio> estudioList = await _estudioRepo.ObtenerTodos(); //Obtiene las Estudios del repositorio
                _response.Resultado = _mapper.Map<IEnumerable<EstudioDto>>(estudioList);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
            //return Ok(await _db.Estudios.ToListAsync());
        }

        [HttpGet("estudioId:int", Name = "GetEstudio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEstudio(int estudioId)
        {
            try
            {
                //Validacion del Id
                if (estudioId <= 0)
                {
                    _logger.LogError("Error al traer estudio con Id " + estudioId);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                //var estudio = await _db.Estudios.FirstOrDefaultAsync(v => v.Id == id);
                var estudio = await _estudioRepo.Obtener(v => v.EstudioID == estudioId);
                if (estudio == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EstudioDto>(estudio);
                _response.StatusCode = HttpStatusCode.OK;
                //return Ok(estudio);
                return Ok(_mapper.Map<EstudioDto>(estudio));
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
        public async Task<ActionResult<APIResponse>> CrearEstudio([FromBody] EstudioCreateDto CreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (CreateDto == null)
                {
                    return BadRequest(CreateDto);
                }
                // Validación específica para StudyInstanceUID
                if (await _estudioRepo.ExisteStudyInstanceUID(CreateDto.StudyInstanceUID))
                {
                    return BadRequest("Ya existe un estudio con el mismo StudyInstanceUID.");
                }
                // Validación de StudyDate
                if (CreateDto.StudyDate.Date > DateTime.Today)
                {
                    return BadRequest("La fecha del estudio no puede ser una fecha futura.");
                }
                Estudio modelo = _mapper.Map<Estudio>(CreateDto); //Mapea y deposita la informacion de CreateDto a modelo
                await _estudioRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetEstudio", new { EstudioID = modelo.EstudioID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{estudioId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEstudio(int estudioId)
        {
            try
            {
                if (estudioId <= 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var estudio = await _estudioRepo.Obtener(v => v.EstudioID == estudioId);
                if (estudio == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _estudioRepo.Remover(estudio);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
                //return NoContent();
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        /// ** UPDATE
        [HttpPut("{estudioId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEstudio(int estudioId, [FromBody] EstudioUpdateDto UpdateDto)
        {
            if (UpdateDto == null || estudioId != UpdateDto.EstudioID)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var estudioExistente = await _estudioRepo.Obtener(x => x.EstudioID == estudioId);
            if (estudioExistente == null)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            Estudio modelo = _mapper.Map<Estudio>(UpdateDto);
            await _estudioRepo.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

        [HttpPatch("{estudioId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialEstudio(int estudioId, JsonPatchDocument<EstudioUpdateDto> patchDto)
        {
            if (patchDto == null || estudioId == 0)
            {
                return BadRequest();
            }
            var estudioExistente = await _estudioRepo.Obtener(x => x.EstudioID == estudioId, tracked: false);
            if (estudioExistente == null)
            {
                return NotFound("Estudio no encontrado.");
            }
            EstudioUpdateDto estudioDto = _mapper.Map<EstudioUpdateDto>(estudioExistente);
            patchDto.ApplyTo(estudioDto, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Estudio modelo = _mapper.Map<Estudio>(estudioDto);
            await _estudioRepo.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
