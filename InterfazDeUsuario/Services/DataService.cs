using InterfazDeUsuario.Models;
using InterfazDeUsuario.Services.IServices;
using System.Net.Http;
using Utileria;

namespace InterfazDeUsuario.Services
{
    public class DataService: BaseService, IDataService
    {
        //Crear Serie
        private readonly IHttpClientFactory _httpClient;
        private string _validateUrl;

        public DataService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _validateUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }
        public Task<T> GetMainList<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _validateUrl + "/api/GeneralService/GetMainList",
                Token = token
            });
        }
    }
}
