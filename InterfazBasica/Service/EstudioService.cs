using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service.IService;
using Utileria;

namespace InterfazBasica.Service
{
    public class EstudioService : BaseService, IEstudioService
    {
        public readonly IHttpClientFactory _httpClient;
        private string _villaUrl;
        public EstudioService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _villaUrl = configuration.GetValue<string>("ServicesUrls:API_URL");
        }
        //
        public Task<T> Actualizar<T>(EstudioUpdateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.PUT,
                Datos = dto,
                Url = _villaUrl + "/api/estudio/"
            });
        }

        public Task<T> Crear<T>(EstudioCreateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _villaUrl + "/api/estudio"
            });
        }

        public Task<T> Obtener<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/estudio/" + id
            });
        }

        public Task<T> ObtenerTodos<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/estudio"
            });
        }

        public Task<T> Remover<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.DELETE,
                Url = _villaUrl + "/api/estudio/" + id
            });
        }
    }
}
