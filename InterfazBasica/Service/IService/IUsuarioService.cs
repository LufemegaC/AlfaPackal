using InterfazBasica_DCStore.Models.Indentity;

namespace InterfazBasica_DCStore.Service.IService
{
    public interface IUsuarioService
    {
        Task<T> Login<T>(LoginRequestDto dto);

        Task<T> Registar<T>(RegistroRequestDto dto);
    }
}
