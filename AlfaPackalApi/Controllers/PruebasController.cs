using AlfaPackalApi.Controllers;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Repositorio.IRepositorio;
using Api_PACsServer.Modelos.Dto;
using AutoMapper;
using FellowOakDicom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api_PACsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PruebasController : ControllerBase
    {
        private readonly ILogger<ImagenController> _logger;
        private readonly IImagenRepositorio _imagenRepo;
        private readonly IPacienteRepositorio _pacienteRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public PruebasController(ILogger<ImagenController> logger, IImagenRepositorio imagenRepo, IPacienteRepositorio pacienteRepo, IMapper mapper)
        {
            _logger = logger;
            _imagenRepo = imagenRepo;
            _pacienteRepo = pacienteRepo;
            _mapper = mapper;
            _response = new APIResponse();
        }


        [HttpPost("CrearImagenP")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public async Task<ActionResult<APIResponse>> CrearImagenP([FromBody] ImagenCreateDto CreateDto)
        {
            try
            {
                if (!ModelState.IsValid || CreateDto == null)
                {
                    return BadRequest(ModelState);
                }
                
                // Validación específica para SOPInstanceUID
                if (await _imagenRepo.ExistBySOPInstanceUID(CreateDto.SOPInstanceUID))
                {
                    return BadRequest("Ya existe un estudio con el mismo SOPInstanceUID.");
                }
                Imagen modelo = _mapper.Map<Imagen>(CreateDto);
                await _imagenRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;
                _response.PacsResourceId = modelo.PACS_ImagenID;
                return CreatedAtRoute("GetImagenByID", new { id = modelo.PACS_ImagenID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorsMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("CrearPacienteP")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CrearPacienteP([FromBody] PacienteCreateDto createDto)
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

    }
}
