using InterfazBasica.Models;
using InterfazBasica_DCStore.Models.Dicom.Web;
using InterfazBasica_DCStore.Models.Dtos.Indentity;
using InterfazBasica_DCStore.Service.DicomServices;
using InterfazBasica_DCStore.Service.IService.Dicom;
using static InterfazBasica_DCStore.Utilities.LocalUtility;

namespace InterfazBasica_DCStore.Service
{
    public class DicomWebService : BaseDicomService , IDicomWebService
    {
        private string _APIUrl;
        // Contexto de Session:
        public readonly IHttpClientFactory _httpClient;
        private string _token; //Token de autorizacion

        public DicomWebService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _APIUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
            _httpClient = httpClient;
        }
        
        public Task<StowRsResponse> RegisterInstances(MultipartFormDataContent content)
        {
            return RegisterInstancesAPI<StowRsResponse>(content, Token);
        }
        // ** METODOS DE REGISTRO DE ENTIDADES ** //
        internal Task<T> RegisterInstancesAPI<T>(MultipartFormDataContent content, string token)
        {
            return DicomSendAsync<T>(new DicomAPIRequest()
            {
                APIType = APIType.POST,
                RequestData = content,
                Url = _APIUrl + "/api/gateway/studies",
                Token = token
            });
        }

        

        // ESTUDIO
        //public async Task<APIResponse> RegistrarEstudio(EstudioCreateDto modelo)
        //{
        //    // Structure valid
        //    var resultValid = IsValidEstudioCreate(modelo);
        //    if (!resultValid.Success)
        //        APIResponse.NoValidEntity(resultValid.ErrorsMessages, modelo);
        //    // Valido si ya existe el registro
        //    var existStudy = await _estudioService.ExistStudyByInstanceUID<APIResponse>(modelo.StudyInstanceUID, Token);
        //    if (existStudy != null && existStudy.Resultado is bool && (bool)existStudy.Resultado)
        //        APIResponse.NoValidEntity(new List<string> { "El paciente ya está registrado con el ID proporcionado." }, modelo);
        //    try
        //    {
        //        var response = await _estudioService.Create<APIResponse>(modelo, Token);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de errores al intentar registrar al paciente.
        //        return APIResponse.NoValidEntity(new List<string> { ex.Message }, modelo);
        //    }
        //}

        internal string Token
        {
            get
            {
                if (string.IsNullOrEmpty(_token))
                {
                    //_token = _httpContextAccessor.HttpContext?.Session.GetString(ServerInfo.SessionToken);
                    _token = Secret.Token;
                }
                return _token;
            }
        }

    }
}
