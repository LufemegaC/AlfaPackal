using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto;
using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;
using static Utileria.DicomUtilities;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudioController : ControllerBase
    {
        private readonly ILogger<EstudioController> _logger;
        // private readonly ApplicationDbContext _db; Antes de implementar "Patron de Repositorio" 
        private readonly IEstudioRepositorio _estudioRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public EstudioController(ILogger<EstudioController> logger, IEstudioRepositorio estudioRepo, IMapper mapper)
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

        [HttpGet("{instanceUID}", Name = "GetStudyByInstanceUID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetStudyByInstanceUID(string instanceUID)
        {
        /*Obtiene estudio por instancia UID*/   
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
                var estudio = await _estudioRepo.GetStudyByInstanceUID(instanceUID);
                if (estudio == null) // Si el registro no fue encontrado
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EstudioDto>(estudio); //Mapeo de informacion 
                _response.StatusCode = HttpStatusCode.OK;
                //return Ok(estudio);
                return Ok(_mapper.Map<EstudioDto>(estudio)); //Retorna al modelo mapeado
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetEstudioByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEstudioByID(int id)
        {
            /* Obtiene estudio por ID ( Control interno ) */
            try
            {
                if (id <= 0)  // Validacion del Id
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                // Localizo registro en el modelo
                var estudio = await _estudioRepo.ObtenerPorID(v => v.EstudioID == id);
                if (estudio == null) // Si el registro no fue encontrado
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EstudioDto>(estudio); //Mapeo de informacion 
                _response.StatusCode = HttpStatusCode.OK; 
                //return Ok(estudio);
                return Ok(_mapper.Map<EstudioDto>(estudio)); //Retorna al modelo mapeado
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
                //Validaciones 
                if (!ModelState.IsValid) // Validacion informacion del modelo
                {
                    return BadRequest(ModelState);
                }
                if (CreateDto == null) // Valido que no sea nulo
                {
                    return BadRequest(CreateDto);
                }
                // Validación específica para StudyInstanceUID
                if (await _estudioRepo.ExisteStudyInstanceUID(CreateDto.StudyInstanceUID))
                {
                    return BadRequest("Ya existe un estudio con el mismo StudyInstanceUID.");
                }
                if(!IsValidDicomUid(CreateDto.StudyInstanceUID))
                {
                    return BadRequest("El formato de la instancia UID no es valido.");
                }
                // Validación de StudyDate
                if (CreateDto.StudyDate.Date > DateTime.Today)
                {
                    return BadRequest("La fecha del estudio no puede ser una fecha futura.");
                }
                Estudio modelo = _mapper.Map<Estudio>(CreateDto); //Mapea y deposita la informacion de CreateDto a modelo
                await _estudioRepo.Crear(modelo); //Impacto en BDs
                //Prepara resupuesta
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;
                //Rotona metodo Get con entidad registrada
                return CreatedAtRoute("GetEstudioByID", new { StudyInstanceUID = modelo.StudyInstanceUID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteEstudio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEstudio(int id)
        {
            try
            {
                // Valido Id
                if (id <= 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                // Identificar registro a eliminar
                var estudio = await _estudioRepo.ObtenerPorID(v => v.EstudioID == id);
                if (estudio == null) //No encontrado
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _estudioRepo.Remover(estudio); //Impacta BDs
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
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEstudio(int id, [FromBody] EstudioUpdateDto UpdateDto)
        {
            try
            {
                // Entidad nulla o con ID distinto
                if (UpdateDto == null || id != UpdateDto.EstudioID)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                // Identificacion de Estudio
                var estudioExistente = await _estudioRepo.ObtenerPorID(x => x.EstudioID == id);
                //No encnotrado
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
            catch(Exception ex)
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
        public async Task<IActionResult> UpdatePartialEstudio(int id, JsonPatchDocument<EstudioUpdateDto> patchDto)
        {
            // Contenido de Json Nullo o sin ID
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            // Indetificacion de estudio a modificar
            var estudioExistente = await _estudioRepo.ObtenerPorID(x => x.EstudioID == id, tracked: false);
            if (estudioExistente == null)
            {
                return NotFound("Estudio no encontrado.");
            }
            // Mapeo a dto de estudio existente
            EstudioUpdateDto estudioDto = _mapper.Map<EstudioUpdateDto>(estudioExistente);
            patchDto.ApplyTo(estudioDto, ModelState); // Modificacion a con Dto
            // Valido entidad modificada
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Mapeamos del Dto al modelo
            Estudio modelo = _mapper.Map<Estudio>(estudioDto);
            await _estudioRepo.Actualizar(modelo); // Impacamos en BDs
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
