using InterfazDeUsuario.Models;
using InterfazDeUsuario.Models.Especificaciones;
using InterfazDeUsuario.Services.IServices;
using System.Net.Http;
using Utileria;

namespace InterfazDeUsuario.Services
{
    public class DataService : BaseService, IDataService
    {
        //Crear Serie
        private readonly IHttpClientFactory _httpClient;
        private string _validateUrl;

        public DataService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _validateUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }
        public Task<T> GetMainList<T>(string token, int institutionId)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _validateUrl + "/api/GeneralService/GetMainList/" + institutionId,
                Token = token
            });
        }

        public Task<T> GetMainListPaginado<T>(string token, int institutionId, int pageNumber = 1, int pageSize = 4)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _validateUrl + "/api/GeneralService/GetMainListPaginado",
                Token = token,
                Parametros = new ParametrosPag() 
                { 
                    PageNumber = pageNumber, 
                    PageSize = pageSize, 
                    InstitutionId = institutionId
                }
            });
        }

        // Método para realizar la llamada al WadoUriController
        public async Task<HttpResponseMessage> GetStudyInstancesAsync(string token, string requestType, string studyUID, string seriesUID, string objectUID, string contentType = null, string charset = null, string transferSyntax = null, string anonymize = null)
        {
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var url = $"{_validateUrl}/wado?requestType={requestType}&studyUID={studyUID}&seriesUID={seriesUID}&objectUID={objectUID}";

            if (!string.IsNullOrEmpty(contentType))
                url += $"&contentType={contentType}";
            if (!string.IsNullOrEmpty(charset))
                url += $"&charset={charset}";
            if (!string.IsNullOrEmpty(transferSyntax))
                url += $"&transferSyntax={transferSyntax}";
            if (!string.IsNullOrEmpty(anonymize))
                url += $"&anonymize={anonymize}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            return await client.SendAsync(request);
        }

    }
}
