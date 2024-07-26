using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto.Pacs;
using Api_PACsServer.Modelos.Dto;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Services.IService.Pacs;
using Api_PACsServer.Utilities;
using AutoMapper;
using System.Net;

namespace Api_PACsServer.Services
{
    public class PacienteService : IPacienteService
    {

        private readonly IPacienteRepositorio _pacienteRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public PacienteService(IPacienteRepositorio pacienteRepo, IMapper mapper)
        {
            _pacienteRepo = pacienteRepo;
            _mapper = mapper;
        }

        public async Task<APIResponse> Create(PacienteCreateDto createDto)
        {
            try
            {
                if(createDto == null)
                    return ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, new List<string> { "objeto nullo." });
                Paciente paciente = _mapper.Map<Paciente>(createDto);
                await _pacienteRepo.Crear(paciente);
                var pacienteDto = _mapper.Map<ImagenDto>(paciente);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Created, pacienteDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Service:" + ex.ToString() });
            }
        }

        public async Task<APIResponse> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    ConverterHelp.CreateResponse(false, HttpStatusCode.BadRequest, new List<string> { "id invalido." });
                Paciente paciente = await _pacienteRepo.Obtener(v => v.PACS_PatientID == id);
                if (paciente == null)
                    ConverterHelp.CreateResponse(false, HttpStatusCode.NotFound, new List<string> { "paciente no encontrado." });
                PacienteDto pacienteDto = _mapper.Map<PacienteDto>(paciente);
                return ConverterHelp.CreateResponse(true, HttpStatusCode.Found, pacienteDto);
            }
            catch (Exception ex)
            {
                return ConverterHelp.CreateResponse(false, HttpStatusCode.InternalServerError, new List<string> { "Service:" + ex.ToString() });
            }
        }

        public PacienteDto MapToDto(Paciente paciente)
        {
            return _mapper.Map<PacienteDto>(paciente);
        }
    }
}
