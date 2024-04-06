using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Dto;
using Api_PACsServer.Services;
using Api_PACsServer.Services.IService;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Api_PACsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateController : ControllerBase
    {
        IValidationService _validationService;
        protected APIResponse _response;


        public ValidateController(IValidationService validationService)
        {
            _validationService = validationService;
            _response = new APIResponse();
        }

        [HttpPost("ValidateEntities")]
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
                mainEntitiesValues = await _validationService.ValidarExistenciaDeEntidades(mainEntitiesValues);
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

    }
}
