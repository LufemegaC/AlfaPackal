using InterfazDeUsuario.Models.Dtos.Identity;

namespace InterfazDeUsuario.Services.IServices
{
    public interface IUserService
    {
        Task<T> Login<T>(LoginRequestDto dto);

        Task<T> Register<T>(RegisterRequestDto dto);
    }
}
