using Api_PACsServer.Models.Dto;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Utilities;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api_PACsServer.Controllers.GatewayData
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly IDicomOrchestrator _orchestrator;
        protected APIResponse _response;
        public GatewayController(IDicomOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            _response = new APIResponse();
        }

        // Crear 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> RegisterEntities([FromBody] MainEntitiesCreateDto createDto)
        {
            try
            {
                //Validaciones 
                if (!ModelState.IsValid || createDto == null) // Validacion informacion del modelo
                    return BadRequest(ModelState);
                var result = await _orchestrator.RegisterMainEntities(createDto); //Impacto en BDs
                if (string.IsNullOrEmpty(result))
                    return ConverterHelp.CreateResponse(true, HttpStatusCode.Created);
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { result });
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { ex.ToString() });
            }
        }


    }
}
