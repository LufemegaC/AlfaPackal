using InterfazDeUsuario.Models;
using InterfazDeUsuario.Models.Dtos.Identity;
using InterfazDeUsuario.Services.IServices;
using static InterfazDeUsuario.Utility.LocalUtility;

namespace InterfazDeUsuario.Services
{
    public class UserService : BaseService, IUserService
    {
        public readonly IHttpClientFactory _httpClient;
        private string _villaUrl;

        public UserService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        public Task<T> Login<T>(LoginRequestDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = APIType.POST,
                RequestData = dto,
                Url = _villaUrl + "/api/user/login"

            });
        }

        public Task<T> Register<T>(RegisterRequestDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = APIType.POST,
                RequestData = dto,
                Url = _villaUrl + "/api/user/register"
            });
        }
    }
}
