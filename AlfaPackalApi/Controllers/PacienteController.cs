using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto;
using AlfaPackalApi.Modelos.Dto.Pacs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Api_PACsServer.Modelos.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Services.IService.Pacs;
using Azure;
using Api_PACsServer.Utilities;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PacienteController : ControllerBase
    {
        private readonly ILogger<PacienteController> _logger;
        private readonly IPacienteService _pacienteService;
        protected APIResponse _response;

        public PacienteController(ILogger<PacienteController> logger, IPacienteService pacienteService)
        {
            _logger = logger;
            _pacienteService = pacienteService;
            _response = new APIResponse();
        }

        // ** Metodos del repositorio ** //
        // ** CREAR
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CrearPaciente([FromBody] PacienteCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid || createDto == null)
                {
                    return BadRequest(ModelState);
                }
                _response = await _pacienteService.Create(createDto);
                // Si algo falla
                if (!_response.IsExitoso)
                    return StatusCode((int)_response.StatusCode, _response);
                // Retorno por ID
                return CreatedAtRoute("GetPacienteByID", new { id = ((PacienteDto)_response.Resultado).PACS_PatientID }, _response);

                //return CreatedAtRoute("GetPacienteByID", new { id = paciente.PACS_PatientID }, _response);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }
        }


        // ** OBTENER TODOS
        // No es necesario por el momento, pendiente eliminars
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<APIResponse>> GetPacientes() //works
        //{
        //    try
        //    {
        //        var listaPacientes = await _pacienteRepo.ObtenerTodos();
        //        _response.Resultado = _mapper.Map<IEnumerable<PacienteDto>>(listaPacientes);
        //        _response.StatusCode = HttpStatusCode.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsExitoso = false;
        //        _response.ErrorsMessages = new List<string> { ex.ToString() };
        //    }
        //    return _response;
        //}

        // ** OBTENER POR ID
        [HttpGet("{id:int}", Name = "GetPacienteByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPacienteByID(int id)
        {
            try
            {
                if (id <= 0)
                    ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, new List<string> { "Id invalido." });
                var _response = await _pacienteService.GetById(id);
                // Si algo falla
                if (!_response.IsExitoso)
                    return StatusCode((int)_response.StatusCode, _response);
                return _response;
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Controller:" + ex.ToString() });
            }
        }

        //** Metodos de Paciente repositorio ** //
        //** GetByName
        //[HttpGet("GetByName/{name}", Name = "GetPacienteByName")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<APIResponse>> GetPacienteByName(string name)
        //{
        //    try
        //    {
        //        if(String.IsNullOrEmpty(name))
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            return BadRequest(_response);
        //        }

        //        var paciente = await _pacienteRepo.GetByName(name);
        //        if (paciente == null)
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }
        //        _response.Resultado = _mapper.Map<PacienteDto>(paciente);
        //        _response.StatusCode = HttpStatusCode.OK;
        //        _response.PacsResourceId = paciente.PACS_PatientID;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsExitoso = false;
        //        _response.ErrorsMessages = new List<string> { ex.ToString() };
        //    }
        //    return _response;
        //}
        // ** GetByGeneratedPatientId
        // GET: api/Paciente/5
        //[HttpGet("GetByGeneratedPatientID/{generatedPatientID}", Name = "GetByGeneratedPatientID")]
        //[Authorize] // Pendiente probar con TOKEN
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<APIResponse>> GetByGeneratedPatientID(string generatedPatientID)
        //{
        //    try
        //    {
        //        if (String.IsNullOrWhiteSpace(generatedPatientID))    
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            return BadRequest(_response);
        //        }

        //        var paciente = await _pacienteRepo.GetByGeneratedPatientId(generatedPatientID);
        //        if (paciente == null)
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }
        //        _response.Resultado = _mapper.Map<PacienteDto>(paciente);
        //        _response.StatusCode = HttpStatusCode.OK;
        //        _response.PacsResourceId = paciente.PACS_PatientID;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsExitoso = false;
        //        _response.ErrorsMessages = new List<string> { ex.ToString() };
        //    }
        //    return _response;
        //}

        //[HttpGet("ExistByMetadata/{patientID}/{issuerOfPatientID}", Name = "ExistPatientByMetadata")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<APIResponse>> ExistByMetadata(string patientID, string issuerOfPatientID)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(patientID) || string.IsNullOrEmpty(issuerOfPatientID))
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            _response.ErrorsMessages = new List<string> { "PatientID and IssuerOfPatientID are required." };
        //            return BadRequest(_response);
        //        }
        //        var exists = await _pacienteRepo.ExistByMetadata(patientID, issuerOfPatientID);
        //        _response.Resultado = exists;
        //        _response.StatusCode = exists ? HttpStatusCode.OK : HttpStatusCode.NotFound;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsExitoso = false;
        //        _response.ErrorsMessages = new List<string> { ex.ToString() };
        //        _response.StatusCode = HttpStatusCode.InternalServerError;
        //        return StatusCode(StatusCodes.Status500InternalServerError, _response);
        //    }
        //}
   
    }
}
