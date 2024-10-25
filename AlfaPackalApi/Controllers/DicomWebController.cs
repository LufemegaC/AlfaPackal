using Api_PACsServer.Models.Dto;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_PACsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DicomWebController : ControllerBase
    {
        private readonly IDicomOrchestrator _orchestrator;
        protected APIResponse _response;

        public DicomWebController(IDicomOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            _response = new APIResponse();
        }

        
    }
}
