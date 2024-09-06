using InterfazBasica.Models;
using InterfazBasica.Service;
using InterfazBasica_DCStore.Models.Dtos.Indentity;
using InterfazBasica_DCStore.Models.Dtos.MainEntities;
using InterfazBasica_DCStore.Service.IService;
using static InterfazBasica_DCStore.Utilities.LocalUtility;

namespace InterfazBasica_DCStore.Service
{
    public class ServiceAPI : BaseService, IServiceAPI
    {
        private string _APIUrl;
        // Contexto de Session:
        public readonly IHttpClientFactory _httpClient;
        private string _token; //Token de autorizacion

        public ServiceAPI(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _APIUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
            _httpClient = httpClient;
        }

        public Task<APIResponse> RegisterMainEntities(MainEntitiesCreateDto CreateDto)
        {
            return RegisterMainEntities<APIResponse>(CreateDto, Token);
        }
        // ** METODOS DE REGISTRO DE ENTIDADES ** //
        internal Task<T> RegisterMainEntities<T>(MainEntitiesCreateDto CreateDto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = APIType.POST,
                RequestData = CreateDto,
                Url = _APIUrl + "/api/gateway/RegisterEntities",
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
