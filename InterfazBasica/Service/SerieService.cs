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
        public Task<T> Create<T>(SerieCreateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _serieUrl + "/api/serie"
            });
        }

        //Get Series
        public Task<T> GetAll<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _serieUrl + "/api/serie"
            });
        }

        public Task<T> GetByID<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _serieUrl + "/api/serie/" + id
            });
        }

        //public Task<T> GetAllByStudyInstanceUID<T>(string uid)
        //{
        //    return SendAsync<T>(new APIRequest()
        //    {
        //        APITipo = DS.APITipo.GET,
        //        Url = _serieUrl + "/api/serie/" + uid
        //    });
        //}

        public Task<T> GetBySeriesInstanceUID<T>(string instanceUID)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _serieUrl + "/api/serie/GetSerieByInstanceUID/" + instanceUID
            });
        } 
        
        public Task<T> ExistByInstanceUID<T>(string instanceUID)
        { 
        return SendAsync<T>(new APIRequest()
        {
            APITipo = DS.APITipo.GET,
                Url = _serieUrl + "/api/serie/ExistSerieByInstanceUID/" + instanceUID
            });
        }
    }
}
