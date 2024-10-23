using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

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
