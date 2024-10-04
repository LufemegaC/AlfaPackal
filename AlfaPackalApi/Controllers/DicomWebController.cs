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

        /// <summary>
        /// Stores DICOM instances using the STOW-RS method.
        /// </summary>
        /// <returns>ActionResult indicating the outcome of the operation.</returns>
        [HttpPost("studies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StoreInstances()
        {
            try
            {
                // Parse the STOW-RS request to get the list of DICOM instances
                var stowRsRequests = await DicomWebHelper.ParseStowRsRequest(Request);

                if (stowRsRequests == null || !stowRsRequests.Any())
                    return BadRequest("No DICOM files were provided.");

                // Register the DICOM instances using the orchestrator
                var operationResults = await _orchestrator.RegisterDicomInstances(stowRsRequests);

                // Create the STOW-RS response using the helper method
                var jsonResponse = DicomWebHelper.CreateStowRsResponse(operationResults);

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
    }
}
