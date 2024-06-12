using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Modelos.Dto;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Api_PACsServer.Modelos.Dto;
using static Utileria.DicomUtilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerieController : ControllerBase
    {
        private readonly ILogger<SerieController> _logger;
        private readonly ISerieRepositorio _serieRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public SerieController(ILogger<SerieController> logger, ISerieRepositorio serieRepo, IMapper mapper)
        {
            _logger = logger;
            _serieRepo = serieRepo;
            _mapper = mapper;
            _response = new APIResponse();
        }


        // POST: api/Serie
        [HttpPost]
        [Authorize] // Pendiente probar con TOKEN
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CrearSerie([FromBody] SerieCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid || createDto == null)
                {
                    return BadRequest(ModelState);
                }
                if (!IsValidDicomUid(createDto.SeriesInstanceUID))
                {
                    return BadRequest("El formato de la instancia UID no es valido.");
                }
                // Validación específica para StudyInstanceUID
                if (await _serieRepo.ExistByInstanceUID(createDto.SeriesInstanceUID))
                {
                    return BadRequest("Ya existe un estudio con el mismo StudyInstanceUID.");
                }
                var serie = _mapper.Map<Serie>(createDto);
                await _serieRepo.Crear(serie);
                _response.Resultado = serie;
                _response.StatusCode = HttpStatusCode.Created;
                _response.PacsResourceId = serie.PACS_SerieID;
                return CreatedAtRoute("GetSerieById", new { id = serie.PACS_SerieID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        // GET: api/Serie
        [HttpGet]
        [Authorize] // Pendiente probar con TOKEN
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetSeries()
        {
            try
            {
                var listaSeries = await _serieRepo.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<SerieDto>>(listaSeries);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        // GET: api/Serie/5
        [HttpGet("{id:int}", Name = "GetSerieById")]
        [Authorize] // Pendiente probar con TOKEN
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetSerieById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var serie = await _serieRepo.ObtenerPorID(v => v.PACS_SerieID == id);
                if (serie == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<SerieDto>(serie);
                _response.StatusCode = HttpStatusCode.OK;
                //return Ok(_response);
                return Ok(_mapper.Map<SerieDto>(serie));
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        
        // GET: api/Serie/5
        [HttpGet("GetSerieByInstanceUID/{instanceUID}", Name = "GetSerieByInstanceUID")]
        [Authorize] // Pendiente probar con TOKEN
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetSerieByInstanceUID(string instanceUID)
        {
            try
            {
                if (!IsValidDicomUid(instanceUID))
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var serie = await _serieRepo.GetByInstanceUID(instanceUID);
                if (serie == null)           
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<SerieDto>(serie);
                _response.StatusCode = HttpStatusCode.OK;
                _response.PacsResourceId = serie.PACS_SerieID;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("ExistSerieByInstanceUID/{instanceUID}", Name = "ExistSerieByInstanceUID")]
        [Authorize] // Pendiente probar con TOKEN
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> ExistSerieByInstanceUID(string instanceUID)
        {
            try
            {
                if (string.IsNullOrEmpty(instanceUID))
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorsMessages = new List<string> { "instanceUID is required." };
                    return BadRequest(_response);
                }
                var exists = await _serieRepo.ExistByInstanceUID(instanceUID);
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

