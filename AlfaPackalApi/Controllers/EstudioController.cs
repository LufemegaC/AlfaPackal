using AlfaPackalApi.Modelos.Dto.Pacs;
using Api_PACsServer.Modelos.Dto;
using Api_PACsServer.Services.IService.Pacs;
using Api_PACsServer.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Utileria.DicomUtilities;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EstudioController : ControllerBase
    {
        private readonly ILogger<EstudioController> _logger;
        // private readonly ApplicationDbContext _db; Antes de implementar "Patron de Repositorio" 
        private readonly IEstudioService _estudioService;
        protected APIResponse _response;

        public EstudioController(ILogger<EstudioController> logger, IEstudioService estudioService)
        {
            _logger = logger;
            _estudioService = estudioService;
            _response = new APIResponse();
        }

        // Crear 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearEstudio([FromBody] EstudioCreateDto createDto)
        {
            try
            {
                //Validaciones 
                if (!ModelState.IsValid || createDto == null) // Validacion informacion del modelo
                    return BadRequest(ModelState);
                var estudioDto = await _estudioService.Crear(createDto); //Impacto en BDs
                if(estudioDto == null)
                    return ConverterHelp.CreateResponse(true, HttpStatusCode.InternalServerError);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Created, estudioDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }
        }

        // Obtener por ID
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
                    ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, new List<string> { "Id invalido." });
                // Localizo registro en el modelo
                var estudioDto = await _estudioService.GetById(id);
                if (estudioDto == null) // Si el registro no fue encontrado
                    return ConverterHelp.CreateResponse(true, HttpStatusCode.NotFound);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Found, estudioDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }
        }

        // ** IEstudioRepositorio ** //
        // Get by study instance UID
        /*Obtiene estudio por instancia UID*/
        [HttpGet("GetStudyByInstanceUID/{instanceUID}", Name = "GetStudyByInstanceUID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetStudyByInstanceUID(string instanceUID)
        {
            try
            {
                //Si el formate de la instancia UID no es valido
                if (!IsValidDicomUid(instanceUID))
                    return ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, null, new List<string> { "UID null o vacio." });
                // Localizo registro en el modelo
                var estudioDto = await _estudioService.GetByUID(instanceUID);
                if (estudioDto == null) // Si el registro no fue encontrado
                    return ConverterHelp.CreateResponse(true, HttpStatusCode.NotFound);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Found, estudioDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }
        }




    }
}
