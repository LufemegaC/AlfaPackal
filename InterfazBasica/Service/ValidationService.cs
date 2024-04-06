using InterfazBasica.Models;
using InterfazBasica.Service;
using InterfazBasica.Service.IService;
using InterfazBasica_DCStore.Models;
using InterfazBasica_DCStore.Service.IService;
using Microsoft.Extensions.Configuration;
using Utileria;

namespace InterfazBasica_DCStore.Service
{
    public class ValidationService: BaseService, IValidationService
    {
        //Crear Serie
        private readonly IHttpClientFactory _httpClient;
        private string _validateUrl;

        public ValidationService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _validateUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        public Task<T> ValidateEntities<T>(MainEntitiesValues mainEntitiesValues)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = mainEntitiesValues,
                Url = _validateUrl + "/api/Validate/ValidateEntities"
            });
        }
    }
}
