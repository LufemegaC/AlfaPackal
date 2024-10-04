using InterfazDeUsuario.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;
using static InterfazDeUsuario.Utility.LocalUtility;

namespace InterfazDeUsuario.Services
{
    public class BaseService
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
                var client = _httpClient.CreateClient("InterfaceClient");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                //Validacion de parametros de paginado
                if (apiRequest.Parameters == null)
                {
                    message.RequestUri = new Uri(apiRequest.Url);
                }
                else
                {
                    var builder = new UriBuilder(apiRequest.Url);
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    //Agregar parametros
                    query["PageNumber"] = apiRequest.Parameters.PageNumber.ToString();
                    query["PageSize"] = apiRequest.Parameters.PageSize.ToString();
                    query["InstitutionId"] = apiRequest.Parameters.InstitutionId.ToString();
                    builder.Query = query.ToString();
                    string url = builder.ToString();
                    message.RequestUri = new Uri(url);
                }
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
                if (!string.IsNullOrEmpty(apiRequest.Token))
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
                    ErrorsMessages = new List<string> { Convert.ToString(ex.Message) },
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
