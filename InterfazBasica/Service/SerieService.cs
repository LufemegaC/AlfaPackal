using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service;
using InterfazBasica_DCStore.Service.IService;
using System;
using Utileria;
using static Utileria.DS;

namespace InterfazBasica_DCStore.Service
{
    public class SerieService : BaseService, ISerieService
    {
        public readonly IHttpClientFactory _httpClient;
        private string _serieUrl;

        public SerieService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _serieUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        //Crear Serie
        public Task<T> Create<T>(SerieCreateDto dto,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _serieUrl + "/api/serie",
                Token = token
            });
        }

        //Get Series
        public Task<T> GetAll<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _serieUrl + "/api/serie",
                Token = token
            });
        }

        public Task<T> GetByID<T>(int id,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _serieUrl + "/api/serie/" + id,
                Token = token
            });
        }

        public Task<T> GetBySeriesInstanceUID<T>(string instanceUID,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _serieUrl + "/api/serie/GetSerieByInstanceUID/" + instanceUID,
                Token = token
            });
        } 
        
        public Task<T> ExistByInstanceUID<T>(string instanceUID,string token)
        { 
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _serieUrl + "/api/serie/ExistSerieByInstanceUID/" + instanceUID,
                Token = token
            });
        }
    }
}
