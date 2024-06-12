using InterfazDeUsuario.Models;

namespace InterfazDeUsuario.Services.IServices
{
    public interface IBaseService
    {
        public APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
