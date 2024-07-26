using AlfaPackalApi.Modelos.Dto.Pacs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Api_PACsServer.Modelos.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Api_PACsServer.Services.IService.Pacs;
using Api_PACsServer.Utilities;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImagenController : ControllerBase
    {
        private readonly ILogger<ImagenController> _logger;
        private readonly IImagenService _imagenService;

        protected APIResponse _response;

        public ImagenController(ILogger<ImagenController> logger, IImagenService imagenService)
        {
            _logger = logger;
            _imagenService = imagenService;
            _response = new APIResponse();
        }

        [HttpGet("{id:int}", Name = "GetImagenByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetImagenById(int id)
        {
            try
            {
                if (id <= 0)
                    ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, new List<string> { "Id invalido." });
                var imagenDto = await _imagenService.GetById(id);
                if (imagenDto == null)
                    return ConverterHelp.CreateResponse(true, HttpStatusCode.NotFound);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Found, imagenDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }      
        }

        [HttpGet("GetImageByInstanceUID/{instanceUID}", Name = "GetImageByInstanceUID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetImageByInstanceUID(string instanceUID)
        {
            try
            {
                //Valido que no venga vacio o null
                if (string.IsNullOrEmpty(instanceUID))
                    return ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, null, new List<string> { "UID null o vacio." });
                // Localizo registro en el modelo
                var imagenDto = await _imagenService.GetByUID(instanceUID);
                if (imagenDto == null)
                    return ConverterHelp.CreateResponse(false, HttpStatusCode.NotFound);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Found, imagenDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }
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
                    return BadRequest(ModelState);
                var imageDto = await _imagenService.Create(CreateDto);
                if (imageDto == null)
                    return ConverterHelp.CreateResponse(true, HttpStatusCode.InternalServerError);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Created, imageDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }
        }
    }
}