using InterfazDeUsuario.Models;
using InterfazDeUsuario.Services.IDicomWeb;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using static InterfazDeUsuario.Utility.LocalUtility;

namespace InterfazDeUsuario.Services
{
    public class DicomWebService : DicomWebBaseService, IDicomWebService
    {
        //Crear Serie
        private readonly IHttpClientFactory _httpClient;
        private string _validateUrl;

        public DicomWebService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _validateUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }


        public Task<JArray> GetMainListPaginado(string token, int limit, int pageNumber, int pageSize)
        {
            // Define a constant "orderby" parameter for simplicity, it will always use "studydate"
            string orderBy = "orderby=studydate";

            // Construct the route with parameters pageNumber, pageSize, limit and orderBy
            string parameters = $"?limit={limit}&{orderBy}&page={pageNumber}&pagesize={pageSize}";

            return DicomSendAsync(new APIRequest()
            {
                APIType = APIType.GET,
                Url = _validateUrl + "/api/Study/studies"+parameters,
                Token = token
            });
        }
    }
}
