using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto;
using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Api_PACsServer.Modelos.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AlfaPackalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly ILogger<PacienteController> _logger;
        private readonly IPacienteRepositorio _pacienteRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public PacienteController(ILogger<PacienteController> logger, IPacienteRepositorio pacienteRepo, IMapper mapper)
        {
            _logger = logger;
            _pacienteRepo = pacienteRepo;
            _mapper = mapper;
            _response = new APIResponse();
        }

        // ** Metodos del repositorio ** //
        // ** CREAR
        [HttpPost]
        [Authorize] // Pendiente probar con TOKEN
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
                var paciente = _mapper.Map<Paciente>(createDto);
                await _pacienteRepo.Crear(paciente);
                _response.Resultado = paciente;
                _response.StatusCode = HttpStatusCode.Created;
                _response.PacsResourceId = paciente.PACS_PatientID;
                _response.GeneratedServId = paciente.GeneratedPatientID;
                return CreatedAtRoute("GetPacienteByID", new { id = paciente.PACS_PatientID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        //[HttpPost("CrearPacientePruebas")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<APIResponse>> CrearPacientePruebas([FromBody] PacienteCreateDto createDto)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid || createDto == null)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        var paciente = _mapper.Map<Paciente>(createDto);
        //        await _pacienteRepo.Crear(paciente);
        //        _response.Resultado = paciente;
        //        _response.StatusCode = HttpStatusCode.Created;
        //        _response.PacsResourceId = paciente.PACS_PatientID;
        //        _response.GeneratedServId = paciente.GeneratedPatientID;
        //        return CreatedAtRoute("GetPacienteByID", new { id = paciente.PACS_PatientID }, _response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsExitoso = false;
        //        _response.ErrorsMessages = new List<string> { ex.ToString() };
        //    }
        //    return _response;
        //}


        // ** OBTENER TODOS
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPacientes() //works
        {
            try
            {
                var listaPacientes = await _pacienteRepo.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<PacienteDto>>(listaPacientes);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        // ** OBTENER POR ID
        [HttpGet("{id:int}", Name = "GetPacienteByID")]
        [Authorize] // Pendiente probar con TOKEN
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPacienteByID(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var paciente = await _pacienteRepo.ObtenerPorID(v => v.PACS_PatientID == id);
                if (paciente == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<PacienteDto>(paciente);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        //** REMOVER
        //[HttpDelete("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> DeletePaciente(int id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            return BadRequest(_response);
        //        }

        //        var paciente = await _pacienteRepo.ObtenerPorID(v => v.PACS_PatientID == id);
        //        if (paciente == null)
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }

        //        await _pacienteRepo.Remover(paciente);
        //        _response.StatusCode = HttpStatusCode.NoContent;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsExitoso = false;
        //        _response.ErrorsMessages = new List<string> { ex.ToString() };
        //    }
        //    return BadRequest(_response);
        //}


        //** Metodos de Paciente repositorio ** //
        //** GetByName
        [HttpGet("GetByName/{name}", Name = "GetPacienteByName")]
        [Authorize] // Pendiente probar con TOKEN
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPacienteByName(string name)
        {
            try
            {
                if(String.IsNullOrEmpty(name))
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var paciente = await _pacienteRepo.GetByName(name);
                if (paciente == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<PacienteDto>(paciente);
                _response.StatusCode = HttpStatusCode.OK;
                _response.PacsResourceId = paciente.PACS_PatientID;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        // ** GetByGeneratedPatientId
        // GET: api/Paciente/5
        [HttpGet("GetByGeneratedPatientID/{generatedPatientID}", Name = "GetByGeneratedPatientID")]
        [Authorize] // Pendiente probar con TOKEN
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetByGeneratedPatientID(string generatedPatientID)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(generatedPatientID))    
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var paciente = await _pacienteRepo.GetByGeneratedPatientId(generatedPatientID);
                if (paciente == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<PacienteDto>(paciente);
                _response.StatusCode = HttpStatusCode.OK;
                _response.PacsResourceId = paciente.PACS_PatientID;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("ExistByMetadata/{patientID}/{issuerOfPatientID}", Name = "ExistPatientByMetadata")]
        [Authorize] // Pendiente probar con TOKEN
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> ExistByMetadata(string patientID, string issuerOfPatientID)
        {
            try
            {
                if (string.IsNullOrEmpty(patientID) || string.IsNullOrEmpty(issuerOfPatientID))
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorsMessages = new List<string> { "PatientID and IssuerOfPatientID are required." };
                    return BadRequest(_response);
                }
                var exists = await _pacienteRepo.ExistByMetadata(patientID, issuerOfPatientID);
                _response.Resultado = exists;
                _response.StatusCode = exists ? HttpStatusCode.OK : HttpStatusCode.NotFound;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
   
    }
}
