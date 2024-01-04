using InterfazBasica.Models;

namespace InterfazBasica.Service.IService
{
    public interface IBaseService
    {
        public APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
