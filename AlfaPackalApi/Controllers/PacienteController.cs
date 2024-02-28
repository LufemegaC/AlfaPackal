using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto;
using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        // GET: api/Paciente
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPacientes()
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

        // GET: api/Paciente/5
        [HttpGet("{id:int}", Name = "GetPaciente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPaciente(int id)
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

        // POST: api/Paciente
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
                var paciente = _mapper.Map<Paciente>(createDto);
                await _pacienteRepo.Crear(paciente);
                _response.Resultado = paciente;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetPaciente", new { id = paciente.PatientID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
   
        // DELETE: api/Paciente/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePaciente(int id)
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

                await _pacienteRepo.Remover(paciente);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        // 18/01/24.-Luis Felipe MG: Codigo comentado en esta version demo
        // PUT: api/Paciente/5
        //[HttpPut("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> UpdatePaciente(int id, [FromBody] PacienteUpdateDto updateDto)
        //{
        //    try
        //    {
        //        if (updateDto == null || id != updateDto.PACS_PatientID)
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            return BadRequest(_response);
        //        }

        //        var pacienteExistente = await _pacienteRepo.ObtenerPorID(x => x.PatientID == id);
        //        if (pacienteExistente == null)
        //        {
        //            _response.IsExitoso = false;
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }

        //        _mapper.Map(updateDto, pacienteExistente);
        //        await _pacienteRepo.Actualizar(pacienteExistente);
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


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialPaciente(int id, JsonPatchDocument<PacienteUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id <= 0)
                {
                    return BadRequest();
                }

                var pacienteExistente = await _pacienteRepo.ObtenerPorID(x => x.PACS_PatientID == id, tracked: false);
                if (pacienteExistente == null)
                {
                    return NotFound("Paciente no encontrado.");
                }

                PacienteUpdateDto pacienteDto = _mapper.Map<PacienteUpdateDto>(pacienteExistente);
                patchDto.ApplyTo(pacienteDto, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _mapper.Map(pacienteDto, pacienteExistente);
                await _pacienteRepo.Actualizar(pacienteExistente);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }


        // Métodos Get, Post, Put, Patch, Delete aquí...
        // Asegúrate de adaptar los métodos para trabajar con la entidad Paciente y sus DTOs correspondientes.
        // Ejemplo: GetPacientes, GetPaciente, CrearPaciente, UpdatePaciente, DeletePaciente, UpdatePartialPaciente
    }
}
