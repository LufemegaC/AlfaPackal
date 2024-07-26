using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Modelos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Api_PACsServer.Modelos.Dto;
using static Utileria.DicomUtilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Api_PACsServer.Modelos.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Services.IService.Pacs;
using Api_PACsServer.Utilities;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SerieController : ControllerBase
    {
        private readonly ILogger<SerieController> _logger;
        private readonly ISerieService _serviceSerie;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public SerieController(ILogger<SerieController> logger, ISerieService serviceSerie, IMapper mapper)
        {
            _logger = logger;
            _serviceSerie = serviceSerie;
            _mapper = mapper;
            _response = new APIResponse();
        }


        // POST: api/Serie
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CrearSerie([FromBody] SerieCreateDto createDto)
        {
            try
            {
                //Validacion de modelo 
                if (!ModelState.IsValid || createDto == null)
                    return BadRequest(ModelState);
                // Creacion de Serie
                var serieDto = await _serviceSerie.Create(createDto);
                if (serieDto == null)
                    return ConverterHelp.CreateResponse(true, HttpStatusCode.InternalServerError);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Created, serieDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }
        }


        // GET: api/Serie/5
        [HttpGet("{id:int}", Name = "GetSerieById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetSerieById(int id)
        {
            try
            {
                if (id <= 0)
                    ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, new List<string> { "Id invalido." });
                var serieDto = await _serviceSerie.GetById(id);
                if (serieDto == null)
                    return ConverterHelp.CreateResponse(true, HttpStatusCode.NotFound);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Found, serieDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }
            
        }
        
        // GET: api/Serie/5
        [HttpGet("GetSerieByInstanceUID/{instanceUID}", Name = "GetSerieByInstanceUID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetSerieByInstanceUID(string instanceUID)
        {
            try
            {
                //Valido que no venga vacio o null
                if (string.IsNullOrEmpty(instanceUID))
                    return ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, new List<string> { "UID null o vacio." });
                var serieDto = await _serviceSerie.GetByUID(instanceUID);
                if (serieDto == null)
                    return ConverterHelp.CreateResponse(true, HttpStatusCode.NotFound);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Found, serieDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }
        }
    }
}

