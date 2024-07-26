using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto.Pacs;
using Api_PACsServer.Modelos.Dto;

namespace Api_PACsServer.Services.IService.Pacs
{
    public interface IPacienteService
    {
        Task<APIResponse> Create(PacienteCreateDto imagen);

        Task<APIResponse> GetById(int id);

        PacienteDto MapToDto(Paciente paciente);
    }
}
