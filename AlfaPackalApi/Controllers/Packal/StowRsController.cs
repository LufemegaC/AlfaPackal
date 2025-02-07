using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.Dto.DicomWeb;
using Api_PACsServer.Models.Dto.DicomWeb.Stow;
using Api_PACsServer.Orchestrators.IOrchestrator;
using Api_PACsServer.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api_PACsServer.Controllers.Packal
{
    [Route("packal/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class StowRsController : ControllerBase
    {
        private readonly IDicomOrchestrator _orchestrator;

        public StowRsController(IDicomOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
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
                var stowRsRequests = await DicomWebHelper.ValidateRequest(Request);

                
                // Register the DICOM instances using the orchestrator
                var operationDicomResults = await _orchestrator.ProcessStowRsRequest(Request, studyInstanceUID);

                // Create the STOW-RS response using the helper method
                //var jsonResponse = DicomWebHelper.CreateStowRsResponse(operationDicomResults, stowRsRequests.TransactionUID);

                // Return the response with the correct content type
                return Content(operationDicomResults, "application/dicom+json");
            }
            catch (Exception ex)
            {
                // Log the exception details (logging code not shown here)
                // For example: _logger.LogError(ex, "An error occurred while processing the STOW-RS request.");

                // Handle exceptions and return an appropriate STOW-RS failure response

                // Create a StowInstanceResult indicating failure
                var failedOperationResult = new StowInstanceResult
                {
                    IsSuccess = false,
                    FailureReason = 272 // Processing failure
                                        // UIDs are unknown in this context, so they remain null
                };

                // Create a FailedInstance using the StowInstanceResult constructor
                var failedInstance = new FailedInstance(failedOperationResult);

                // Create the StowRsResponse using the new constructor
                var failedResponse = new StowRsResponse(
                    acceptedInstances: new List<ReferencedSOPInstance>(), // Empty list since no instances were accepted
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
