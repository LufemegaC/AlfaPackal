using Api_PACsServer.Modelos.Dto;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Services.IService;
using Api_PACsServer.Services.IService.FrontendData;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api_PACsServer.Controllers.FrontEndData
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPortalController : ControllerBase
    {
        private readonly IStudiesPatientService _studiesPatientService;
        protected APIResponse _response;

        public UserPortalController(IStudiesPatientService studiesPatientService)
        {
            _studiesPatientService = studiesPatientService;
            _response = new APIResponse();
        }

        [HttpGet("GetMainListPaginado")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<APIResponse> GetMainListPaginado([FromQuery] ParametrosPag parametros)
        /*Metodo para obtener todas las villas*/
        {
            try
            {
                var studyList = _studiesPatientService.GetMainStudiesList(parametros);
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
