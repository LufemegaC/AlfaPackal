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

                var studyDtos = await _orchestrator.GetInfoStudy(studyParamsDto, controlParamsDto);

                var jsonResponse = DicomWebHelper.ConvertStudiesToDicomJsonString(studyDtos);

                return Content(jsonResponse, "application/dicom+json");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, $"Error del servidor: {ex.Message}");
            }
        }

        ///// <summary>
        ///// Handles GET requests for retrieving series based on provided query parameters.
        ///// </summary>
        ///// <param name="studyInstanceUID">The Study Instance UID to filter series by.</param>
        ///// <param name="queryParams">Dictionary of query parameters to filter the data by.</param>
        ///// <returns>JSON response with the list of series.</returns>
        //[HttpGet("qido-rs/studies/{studyInstanceUID}/series")]
        //public async Task<IActionResult> GetSeries(string studyInstanceUID, [FromQuery] Dictionary<string, string> queryParams)
        //{
        //    try
        //    {
        //        // Añadir StudyInstanceUID a los parámetros
        //        queryParams["StudyInstanceUID"] = studyInstanceUID;

        //        // Dividir los parámetros en DTOs correspondientes
        //        var (controlParamsDto, seriesParamsDto) = DicomWebHelper.MapQueryParamsToDtos(queryParams);

        //        var seriesDtos = await _orchestrator.GetInfoSeries(seriesParamsDto, controlParamsDto);

        //        var jsonResponse = DicomWebHelper.ConvertSeriesToDicomJsonString(seriesDtos);

        //        return Content(jsonResponse, "application/dicom+json");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de excepciones
        //        return StatusCode(500, $"Error del servidor: {ex.Message}");
        //    }
        //}

        ///// <summary>
        ///// Handles GET requests for retrieving instances based on provided query parameters.
        ///// </summary>
        ///// <param name="studyInstanceUID">The Study Instance UID to filter instances by.</param>
        ///// <param name="seriesInstanceUID">The Series Instance UID to filter instances by.</param>
        ///// <param name="queryParams">Dictionary of query parameters to filter the data by.</param>
        ///// <returns>JSON response with the list of instances.</returns>
        //[HttpGet("qido-rs/studies/{studyInstanceUID}/series/{seriesInstanceUID}/instances")]
        //public async Task<IActionResult> GetInstances(string studyInstanceUID, string seriesInstanceUID, [FromQuery] Dictionary<string, string> queryParams)
        //{
        //    try
        //    {
        //        // Añadir StudyInstanceUID y SeriesInstanceUID a los parámetros
        //        queryParams["StudyInstanceUID"] = studyInstanceUID;
        //        queryParams["SeriesInstanceUID"] = seriesInstanceUID;

        //        // Dividir los parámetros en DTOs correspondientes
        //        var (controlParamsDto, instanceParamsDto) = DicomWebHelper.MapQueryParamsToDtos(queryParams);

        //        var instanceDtos = await _orchestrator.GetInfoInstance(instanceParamsDto, controlParamsDto);

        //        var jsonResponse = DicomWebHelper.ConvertInstancesToDicomJsonString(instanceDtos);

        //        return Content(jsonResponse, "application/dicom+json");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de excepciones
        //        return StatusCode(500, $"Error del servidor: {ex.Message}");
        //    }
        //}
    }
}
