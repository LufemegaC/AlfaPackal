using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Repositorio.IRepositorio;
using Api_PACsServer.Modelos.Dto;
using AutoMapper;
using FellowOakDicom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Utileria.DicomUtilities;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Pendiente probar con TOKEN
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
            _response = new APIResponse();
        }

        // Crear 
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
                    return BadRequest(ModelState);
                if (CreateDto == null) // Valido que no sea nulo
                    return BadRequest(CreateDto);
                // Validación específica para StudyInstanceUID
                if (await _estudioRepo.ExistByInstanceUID(CreateDto.StudyInstanceUID))
                    return BadRequest("Ya existe un estudio con el mismo StudyInstanceUID.");
                if (!DicomUID.IsValidUid(CreateDto.StudyInstanceUID))
                    return BadRequest("El formato de la instancia UID no es valido.");
                // Validación de StudyDate
                if (CreateDto.StudyDate > DateTime.Today)
                    return BadRequest("La fecha del estudio no puede ser una fecha futura.");
                Estudio modelo = _mapper.Map<Estudio>(CreateDto); //Mapea y deposita la informacion de CreateDto a modelo
                await _estudioRepo.Crear(modelo); //Impacto en BDs
                //Prepara resupuesta
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;
                _response.PacsResourceId = modelo.PACS_EstudioID;
                //Rotona metodo Get con entidad registrada
                return CreatedAtRoute("GetEstudioByID", new { id = modelo.PACS_EstudioID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        //Obtener todos
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        /*[Authorize] // Pruebas*/
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

        // Obtener por ID
        [HttpGet("{id:int}", Name = "GetEstudioByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize] // Pendiente probar con TOKEN
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
                var estudio = await _estudioRepo.ObtenerPorID(v => v.PACS_EstudioID == id);
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

        [HttpDelete("{id:int}", Name = "DeleteEstudio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize] // Pendiente probar con TOKEN
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
                var estudio = await _estudioRepo.ObtenerPorID(v => v.PACS_EstudioID == id);
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

        // ** IEstudioRepositorio ** //
        // Get by study instance UID
        /*Obtiene estudio por instancia UID*/
        [HttpGet("GetStudyByInstanceUID/{instanceUID}", Name = "GetStudyByInstanceUID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize] // Pendiente probar con TOKEN
        public async Task<ActionResult<APIResponse>> GetStudyByInstanceUID(string instanceUID)
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
                var estudio = await _estudioRepo.GetByInstanceUID(instanceUID);
                if (estudio == null) // Si el registro no fue encontrado
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EstudioDto>(estudio); //Mapeo de informacion 
                _response.StatusCode = HttpStatusCode.OK;
                _response.PacsResourceId = estudio.PACS_EstudioID;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        // Get Study by Accession Number
        [HttpGet("GetbyAccessionNumber/{accessionNumber}", Name = "GetStudyByAccessionNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize] // Pendiente probar con TOKEN
        public async Task<ActionResult<APIResponse>> GetStudyByAccessionNumber (string accessionNumber)
        {
            try
            {
                //Si el formate de la instancia UID no es valido
                if (!IsValidDicomUid(accessionNumber))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                // Localizo registro en el modelo
                var estudio = await _estudioRepo.GetByAccessionNumber(accessionNumber);
                if (estudio == null) // Si el registro no fue encontrado
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EstudioDto>(estudio); //Mapeo de informacion 
                _response.StatusCode = HttpStatusCode.OK;
                _response.PacsResourceId = estudio.PACS_EstudioID;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("ExistStudyByUID/{instanceUID}", Name = "ExistStudyByUID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize] // Pendiente probar con TOKEN
        public async Task<ActionResult<APIResponse>> ExistStudyByInstanceUID(string instanceUID)
        {
            try
            {
                if (string.IsNullOrEmpty(instanceUID))
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorsMessages = new List<string> { "instanceUIDD is required." };
                    return BadRequest(_response);
                }
                var exists = await _estudioRepo.ExistByInstanceUID(instanceUID);
                _response.Resultado = exists;
                _response.StatusCode = exists ? HttpStatusCode.OK : HttpStatusCode.NotFound;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
 

    }
}
