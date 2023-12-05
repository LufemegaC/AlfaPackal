using AlfaPackalApi.Modelos.Dto.Roles;
using AlfaPackalApi.Modelos.Dto;
using AlfaPackalApi.Modelos;
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
    public class DoctorController : ControllerBase
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly IDoctorRepositorio _doctorRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public DoctorController(ILogger<DoctorController> logger, IDoctorRepositorio doctorRepo, IMapper mapper)
        {
            _logger = logger;
            _doctorRepo = doctorRepo;
            _mapper = mapper;
            _response = new APIResponse();
        }

        // GET: api/Doctor
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetDoctores()
        {
            try
            {
                var listaDoctores = await _doctorRepo.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<DoctorDto>>(listaDoctores);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        // GET: api/Doctor/5
        [HttpGet("{id:int}", Name = "GetDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetDoctorID(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var doctor = await _doctorRepo.ObtenerPorID(v => v.DoctorID == id);
                if (doctor == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<DoctorDto>(doctor);
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

        // POST: api/Doctor
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CrearDoctor([FromBody] DoctorCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid || createDto == null)
                {
                    return BadRequest(ModelState);
                }
                var doctor = _mapper.Map<Doctor>(createDto);
                await _doctorRepo.Crear(doctor);
                _response.Resultado = doctor;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetDoctor", new { id = doctor.DoctorID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        // PUT: api/Doctor/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.DoctorID)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var doctorExistente = await _doctorRepo.ObtenerPorID(x => x.DoctorID == id);
                if (doctorExistente == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _mapper.Map(updateDto, doctorExistente);
                await _doctorRepo.Actualizar(doctorExistente);
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

        // DELETE: api/Doctor/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var doctor = await _doctorRepo.ObtenerPorID(v => v.DoctorID == id);
                if (doctor == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _doctorRepo.Remover(doctor);
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

        // PATCH: api/Doctor/5
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialDoctor(int id, JsonPatchDocument<DoctorUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id <= 0)
                {
                    return BadRequest();
                }

                var doctorExistente = await _doctorRepo.ObtenerPorID(x => x.DoctorID == id, tracked: false);
                if (doctorExistente == null)
                {
                    return NotFound("Doctor no encontrado.");
                }

                DoctorUpdateDto doctorDto = _mapper.Map<DoctorUpdateDto>(doctorExistente);
                patchDto.ApplyTo(doctorDto, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _mapper.Map(doctorDto, doctorExistente);
                await _doctorRepo.Actualizar(doctorExistente);
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
    }
}
