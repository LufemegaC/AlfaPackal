using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Dto;
using Api_PACsServer.Services;
using Api_PACsServer.Services.IService;
using Azure;
using FellowOakDicom.Network;
using FellowOakDicom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Api_PACsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralServiceController : ControllerBase
    {
        IGeneralAPIService _generalAPIService;
        protected APIResponse _response;


        public GeneralServiceController(IGeneralAPIService generalAPIService)
        {
            _generalAPIService = generalAPIService;
            _response = new APIResponse();
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
        [Authorize]
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

        [HttpGet("GetMainList")]
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


        
    }
}
