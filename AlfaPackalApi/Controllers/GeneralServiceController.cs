using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Dto;
using Api_PACsServer.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Modelos.AccessControl;
using AutoMapper;
using Api_PACsServer.Modelos.Dto.Vistas;

namespace Api_PACsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GeneralServiceController : ControllerBase
    {
        private readonly IGeneralAPIService _generalAPIService;
        protected APIResponse _response;
        private readonly IMapper _mapper;


        public GeneralServiceController(IGeneralAPIService generalAPIService, IMapper mapper)
        {
            _generalAPIService = generalAPIService;
            _response = new APIResponse();
            _mapper = mapper;
        }

        [HttpPost("ValidateEntities")]
        //[Authorize] // Pruebas
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> ValidateEntities([FromBody] MainEntitiesValues mainEntitiesValues)
        {
            try
            {
                if (mainEntitiesValues == null)
                {
                    return BadRequest("El objeto de validación es nulo");
                }
                mainEntitiesValues = await _generalAPIService.ValidarExistenciaDeEntidades(mainEntitiesValues);
                //_response.Resultado = mainEntitiesValues;
                _response.ResultadoJson = JsonConvert.SerializeObject(mainEntitiesValues);
                //_response.Resultado = Newtonsoft.Json.JsonSerializer.Serialize(MainEntitiesValues);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("ListadoPrincipal")]
        public async Task<ActionResult<APIResponse>> ObtenerListadoPrincipal()
        {
            try
            {
                var estudiosConPacientes = await _generalAPIService.ObtenerEstudiosConPaciente();
                _response.Resultado = estudiosConPacientes;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("GetMainList/{institutionId:int}", Name = "GetMainList")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMainList(int institutionId)
        {
            try
            {
                var estudiosConPacientes = await _generalAPIService.GetStudyList(institutionId);
                _response.Resultado = estudiosConPacientes;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("GetMainListPaginado")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<APIResponse> GetMainListPaginado([FromQuery] ParametrosPag parametros)
        /*Metodo para obtener todas las villas*/
        {
            try
            {
                var studyList = _generalAPIService.ListaEstudiosPaginado(parametros);
                _response.Resultado = studyList;
                _response.StatusCode = HttpStatusCode.OK;
                _response.TotalPaginas = studyList.MetaData.TotalPages;
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
            //return Ok(await _db.Villas.ToListAsync());
        }
    }
}
