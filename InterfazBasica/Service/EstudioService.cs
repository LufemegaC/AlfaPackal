using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service.IService;
using Utileria;

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

        public Task<T> Create<T>(EstudioCreateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _estudioUrl + "/api/estudio"
            });
        }

        public Task<T> GetAll<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio"
            });
        }

        public Task<T> GetByID<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio/" + id
            });
        }

        public Task<T> GetAllByDate<T>(DateTime date)
        {
            string dateString = date.ToString("yyyy-MM-dd");
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio/" + dateString
            });
        }

        public Task<T> GetStudyByInstanceUID<T>(string instanceUID)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio/GetStudyByInstanceUID/" + instanceUID
            });
        }

        public Task<T> GetStudyByAccessionNumber<T>(string accessionNumber)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio/GetbyAccessionNumber/" + accessionNumber
            });
        }

        public Task<T> ExistStudyByInstanceUID<T>(string instanceUID)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _estudioUrl + "/api/estudio/ExistStudyByUID/" + instanceUID
            });
        }
    }
}
