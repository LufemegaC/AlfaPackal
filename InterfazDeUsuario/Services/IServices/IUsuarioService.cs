using InterfazDeUsuario.Models.Identity;

namespace InterfazDeUsuario.Services.IServices
{
    public interface IUsuarioService
    {
        Task<T> Login<T>(LoginRequestDto dto);

        Task<T> Registar<T>(RegistroRequestDto dto);
    }
}
