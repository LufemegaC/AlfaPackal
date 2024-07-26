using InterfazDeUsuario.Services.IServices;
using System.Net.Http;

namespace InterfazDeUsuario.Services
{
    public class WadoUriService : BaseService, IWadoUriService
    {
        private readonly IHttpClientFactory _httpClient;
        private string _validateUrl;

        public WadoUriService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _validateUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

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
        
        
        public async Task<HttpResponseMessage> GetInstancesByStudyUIDAsync(string token, string requestType, string studyUID)
        {
            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var url = $"{_validateUrl}/wado?requestType={requestType}&studyUID={studyUID}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return await client.SendAsync(request);
        }

        // Método para realizar la llamada al WadoUriController

    }
}
