using Api_PACsServer.Models.Dto;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Api_PACsServer.Controllers.FrontEndData
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PhysicianController : ControllerBase
    {
        private readonly IDicomOrchestrator _orchestrator;

        protected APIResponse _response;
        public PhysicianController(IDicomOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            _response = new APIResponse();
        }

        //[HttpGet("ListStudies")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public ActionResult<APIResponse> GetMainStudiesList([FromQuery] PaginationParameters parameters)
        ///*Metodo para obtener todas las villas*/
        //{
        //    try
        //    {
        //        var studiesList = _orchestrator.GetRecentStudies(parameters);
        //        _response.ResponseData = studiesList.Studies;
        //        _response.StatusCode = HttpStatusCode.OK;
        //        _response.TotalPages = studiesList.TotalPages;
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccessful = false;
        //        _response.ErrorsMessages = new List<string> { ex.ToString() };
        //    }
        //    return _response;
        //}

        //[HttpGet("GetInfoStudy/{instanceUID}", Name = "GetInfoStudy")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<APIResponse>> GetInfoStudy(string instanceUID)
        //{
        //    /* Obtiene estudio por ID ( Control interno ) */
        //    try
        //    {
        //        if(string.IsNullOrEmpty(instanceUID))
        //        {
        //            _response.IsSuccessful = false;
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            _response.ErrorsMessages = new List<string> {"UID invalido"};
        //        }
        //        // Localizo registro en el modelo
        //        var studyinfo = await _orchestrator.GetInfoStudy(instanceUID);
        //        return ConverterHelp.ConvertToApiResponse(studyinfo);
        //        //if (studyinfo == null) // Si el registro no fue encontrado
        //        //{
        //        //    _response.StatusCode = HttpStatusCode.NotFound;
        //        //    _response.IsSuccessful = false;
        //        //    return NotFound(_response);
        //        //}
        //        //_response.ResponseData = studyinfo; //Mapeo de informacion 
        //        //_response.StatusCode = HttpStatusCode.OK;
        //        ////return Ok(estudio);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccessful = false;
        //        _response.ErrorsMessages = new List<string> { ex.ToString() };
        //    }
        //    return _response;
        //}
    }
}
