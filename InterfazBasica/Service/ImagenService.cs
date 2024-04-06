using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service;
using InterfazBasica_DCStore.Service.IService;
using Utileria;

namespace InterfazBasica_DCStore.Service
{
    public class ImagenService: BaseService, IImageService
    {
        public readonly IHttpClientFactory _httpClient;
        private string _imagenUrl;

        public ImagenService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _imagenUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        public Task<T> Create<T>(ImagenCreateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _imagenUrl + "/api/imagen/CrearImagen"
            });
        }

        public Task<T> GetbyID<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _imagenUrl + "/api/imagen/" + id
            });
        }

        public Task<T> GetbySOPInstanceUID<T>(string uid)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _imagenUrl + "/api/imagen/GetImageByInstanceUID/" + uid
            });
        }

        //public Task<T> GetAllByStudyInstanceUID<T>(string uid)
        //{
        //    return SendAsync<T>(new APIRequest()
        //    {
        //        APITipo = DS.APITipo.GET,
        //        Url = _imagenUrl + "/api/imagen/" + uid
        //    });
        //}

        public Task<T> GetAll<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _imagenUrl + "/api/imagen"
            });
        }


        public Task<T> ExistBySOPInstanceUID<T>(string sopInstanceUID)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _imagenUrl + "/api/imagen/ExistBySOPInstanceUID/" + sopInstanceUID
            });
        }
    }
}
