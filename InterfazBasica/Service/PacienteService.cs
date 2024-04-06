using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service;
using InterfazBasica_DCStore.Service.IService;
using Utileria;

namespace InterfazBasica_DCStore.Service
{
    public class PacienteService : BaseService, IPacienteService
    {
        public readonly IHttpClientFactory _httpClient;
        private string _pacienteUrl;

        public PacienteService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _pacienteUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }

        //CrearPaciente
        public Task<T> Create<T>(PacienteCreateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _pacienteUrl + "/api/paciente"
            });
        }

        ////CrearPacientePruebas
        //public Task<T> CreatePruebas<T>(PacienteCreateDto dto)
        //{
        //    return SendAsync<T>(new APIRequest()
        //    {
        //        APITipo = DS.APITipo.POST,
        //        Datos = dto,
        //        Url = _pacienteUrl + "/api/paciente/CrearPacientePruebas"
        //    });
        //}

        //GetPaciente
        public Task<T> GetAll<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _pacienteUrl + "/api/paciente"
            });
        }

        //GetPacienteByID
        public Task<T> GetByPACS_ID<T>(int pacsId)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _pacienteUrl + "/api/paciente/" + pacsId
            });
        }

        //DeletePaciente - no implementada

        //Metodos de Paciente repo
        public Task<T> GetByName<T>(string name)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _pacienteUrl + "/api/paciente/GetByName/" + name
            });
        }

        public Task<T> GetByGeneratedPatientID<T>(string generatedId)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _pacienteUrl + "/api/paciente/GetByGeneratedPatientID/" + generatedId
            });
        }

        public Task<T> ExistByMetadata<T>(string patientID, string issuerOfPatientID)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _pacienteUrl+"/api/paciente/ExistByMetadata/"+patientID+"/"+issuerOfPatientID
            });
        }
  
    }
}
