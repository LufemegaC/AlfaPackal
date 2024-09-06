using InterfazBasica.Models;
using InterfazBasica.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static InterfazBasica_DCStore.Utilities.LocalUtility;

namespace InterfazBasica.Service
{
    public class BaseService : IBaseService

    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            _httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("dicomAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                //Validacion de contenido
                if (apiRequest.RequestData != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.RequestData),
                    Encoding.UTF8, "application/json");
                }
                //Configuracion del tipo de solicitud a realizar
                switch (apiRequest.APIType)
                {
                    case APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null; // Inicializo respuesta
                if(!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }
                apiResponse = await client.SendAsync(message); // Envio mensaje
                var apiContent = await apiResponse.Content.ReadAsStringAsync(); //Recibo respuesta
                try
                {
                    APIResponse response = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (response != null && (apiResponse.StatusCode == HttpStatusCode.BadRequest 
                                          || apiResponse.StatusCode == HttpStatusCode.NotFound))
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccessful = false;
                        var res = JsonConvert.SerializeObject(response);
                        var obj = JsonConvert.DeserializeObject<T>(res);
                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    var errorResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return errorResponse;
                }
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent); //Convierto respuesta
                return APIResponse; // Retorno resultado

            }
            catch (Exception ex)
            {// En caso de errores
                var dto = new APIResponse
                {
                    ErrorsMessages = new List<string> { Convert.ToString(ex.Message)},
                    IsSuccessful = false
                };
                // Conversion de respuesta
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;


            }
        }
    }
}
