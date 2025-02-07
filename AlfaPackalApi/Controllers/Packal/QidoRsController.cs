using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Utilities;
using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_PACsServer.Controllers.Packal
{
    [Route("packal/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QidoRsController : ControllerBase
    {
        private readonly IDicomOrchestrator _orchestrator;
        public QidoRsController(IDicomOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpGet("studies")]
        public async Task<IActionResult> GetStudies()
        {
            try
            {
                // Leer los parámetros directamente del request
                var queryParams = Request.Query;

                // Delegar la lógica al orchestrator
                var jsonResponse = await _orchestrator.GetAllStudies(queryParams);

                // Devolver el resultado como JSON
                return Content(jsonResponse, "application/dicom+json");
            }
            catch(Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles GET requests for retrieving series based on provided query parameters.
        /// </summary>
        /// <param name="studyInstanceUID">The Study Instance UID to filter series by.</param>
        /// <returns>JSON response with the list of series.</returns>
        [HttpGet("studies/{studyInstanceUID}/series")]
        public async Task<IActionResult> GetSeries(string studyInstanceUID)
        {
            try
            {
                // Leer los parámetros directamente del request
                var queryParams = Request.Query;
                var jsonResponse = await _orchestrator.GetAllSeriesFromStudy(studyInstanceUID, queryParams);
                return Content(jsonResponse, "application/dicom+json");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
        }


        /// <summary>
        /// Handles GET requests for retrieving instances based on provided query parameters.
        /// </summary>
        /// <param name="studyInstanceUID">The Study Instance UID to filter instances by.</param>
        /// <param name="seriesInstanceUID">The Series Instance UID to filter instances by.</param>
        /// <returns>JSON response with the list of instances.</returns>
        [HttpGet("studies/{studyInstanceUID}/series/{seriesInstanceUID}/instances")]
        public async Task<IActionResult> GetInstances(string studyInstanceUID, string seriesInstanceUID)
        {
            try
            {
                // Leer los parámetros directamente del request
                var queryParams = Request.Query;

                // Delegar la lógica al orchestrator
                var jsonResponse = await _orchestrator.GetAllInstancesFromSeries(studyInstanceUID, seriesInstanceUID, queryParams);

                // Devolver el resultado como JSON con el MIME type adecuado
                return Content(jsonResponse, "application/dicom+json");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
        }
    }
}
