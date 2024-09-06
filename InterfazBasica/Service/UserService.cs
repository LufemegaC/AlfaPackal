using InterfazBasica.Models;
using InterfazBasica.Service;
using InterfazBasica_DCStore.Models.Dtos.Indentity;
using InterfazBasica_DCStore.Service.IService;
using static InterfazBasica_DCStore.Utilities.LocalUtility;

namespace InterfazBasica_DCStore.Service
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
    }
}
