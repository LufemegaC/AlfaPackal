using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api_PACsServer.Controllers.GatewayData
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StudyController : ControllerBase
    {
        private readonly IDicomOrchestrator _orchestrator;
        protected APIResponse _response;
        public StudyController(IDicomOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
            _response = new APIResponse();
        }

        /// <summary>
        /// Stores DICOM instances using the STOW-RS method.
        /// </summary>
        [HttpPost("studies/{studyInstanceUID?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StoreInstances(string studyInstanceUID = null)
        {
            try
            {
                // Parse the STOW-RS request to get the list of DICOM instances
                var stowRsRequests = await DicomWebHelper.ParseStowRsRequest(Request);

                if (stowRsRequests == null || !stowRsRequests.DicomFilesPackage.Any())
                    return BadRequest("No DICOM files were provided.");

                // Register the DICOM instances using the orchestrator
                var operationDicomResults = await _orchestrator.RegisterDicomInstances(stowRsRequests.DicomFilesPackage, studyInstanceUID);

                // Create the STOW-RS response using the helper method
                var jsonResponse = DicomWebHelper.CreateStowRsResponse(operationDicomResults, stowRsRequests.TransactionUID);

                // Return the response with the correct content type
                return Content(jsonResponse, "application/dicom+json");
            }
            catch (Exception ex)
            {
                // Log the exception details (logging code not shown here)
                // For example: _logger.LogError(ex, "An error occurred while processing the STOW-RS request.");

                // Handle exceptions and return an appropriate STOW-RS failure response

                // Create a DicomOperationResult indicating failure
                var failedOperationResult = new DicomOperationResult
                {
                    IsSuccess = false,
                    FailureReason = 272 // Processing failure
                                        // UIDs are unknown in this context, so they remain null
                };

                // Create a FailedInstance using the DicomOperationResult constructor
                var failedInstance = new FailedInstance(failedOperationResult);

                // Create the StowRsResponse using the new constructor
                var failedResponse = new StowRsResponse(
                    acceptedInstances: new List<AcceptedInstance>(), // Empty list since no instances were accepted
                    failedInstances: new List<FailedInstance> { failedInstance }
                );

                // Serialize the response to JSON
                var jsonResponse = JsonConvert.SerializeObject(failedResponse);

                // Return the response with the appropriate status code and content type
                return StatusCode(StatusCodes.Status500InternalServerError, jsonResponse);
            }
        }




        /// <summary>
        /// Handles GET requests for retrieving studies based on provided query parameters.
        /// </summary>
        /// <param name="queryParams">Dictionary of query parameters to filter studies by.</param>
        /// <returns>JSON response with the list of studies.</returns>
        [HttpGet("studies")]
        public async Task<IActionResult> GetStudies([FromQuery] Dictionary<string, string> queryParams)
        {
            try
            {
                // Dividir los parámetros en DTOs correspondientes
                var (controlParamsDto, studyParamsDto) = DicomWebHelper.MapQueryParamsToDtos(queryParams);


                // Aquí se procesaría la solicitud con base en los parámetros recibidos
                // Por ejemplo, podrías crear una lógica para buscar en tu base de datos o interactuar con otro servicio

                // Este es un ejemplo de respuesta simulada
                var responseData = new List<object>
                {
                    new { StudyInstanceUID = "1.2.840.113619.2.55.3.604688084.233.1566038400.467", Parameters = requestData }
                };

                //return Json(responseData);
                return null;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
        }
    }
    
}
