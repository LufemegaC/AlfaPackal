using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service.IService;
using System;
using Utileria;
using static Utileria.DS;

namespace InterfazBasica.Service
{
    public class EstudioService : BaseService, IEstudioService
    {
        public readonly IHttpClientFactory _httpClient;
        private string _estudioUrl;
        public EstudioService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _estudioUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        public Task<T> Create<T>(EstudioCreateDto dto,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _estudioUrl + "/api/estudio",
                Token = token
            });
        }

        public Task<T> GetAll<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio" ,
                Token = token
            });
        }

        public Task<T> GetByID<T>(int id,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio/" + id,
                Token = token
            });
        }

        public Task<T> GetAllByDate<T>(DateTime date,string token)
        {
            string dateString = date.ToString("yyyy-MM-dd");
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio/" + dateString,
                Token = token
            });
        }

        public Task<T> GetStudyByInstanceUID<T>(string instanceUID,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio/GetStudyByInstanceUID/" + instanceUID,
                Token = token
            });
        }

        public Task<T> GetStudyByAccessionNumber<T>(string accessionNumber,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio/GetbyAccessionNumber/" + accessionNumber,
                Token = token
            });
        }

        public Task<T> ExistStudyByInstanceUID<T>(string instanceUID,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio/ExistStudyByUID/" + instanceUID,
                Token = token
            });
        }
    }
}
