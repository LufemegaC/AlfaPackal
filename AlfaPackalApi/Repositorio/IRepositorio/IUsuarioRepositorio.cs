using Api_PACsServer.Modelos.Dto;
using Api_PACsServer.Modelos;

namespace Api_PACsServer.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        bool IsUsarioUnico(string userName);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<UsuarioDto> Registrar(RegistroRequestDto registroRequestDto);
    }
}
