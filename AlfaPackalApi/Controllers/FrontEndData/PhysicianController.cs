using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api_PACsServer.Controllers.FrontEndData
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PhysicianController : ControllerBase
    {
        private readonly IUserOchestrator _userOrchestrator;
        
        protected APIResponse _response;
        public PhysicianController(IUserOchestrator userOrchestrator)
        {
            _userOrchestrator = userOrchestrator;
            _response = new APIResponse();
        }

        [HttpGet("ListStudies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<APIResponse> GetMainStudiesList([FromQuery] PaginationParameters parameters)
        /*Metodo para obtener todas las villas*/
        {
            try
            {
                var studiesList = _userOrchestrator.GetRecentStudies(parameters);
                _response.ResponseData = studiesList.Studies;
                _response.StatusCode = HttpStatusCode.OK;
                _response.TotalPages = studiesList.TotalPages;
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
