﻿using DicomProcessingService.Dtos;
using DicomProcessingService.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;
using Utileria;

namespace DicomProcessingService.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            responseModel = new();
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
                if (apiRequest.Datos != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Datos),
                        Encoding.UTF8, "application/json");
                }
                //Configuracion del tipo de solicitud a realizar
                switch (apiRequest.APITipo)
                {
                    case DS.APITipo.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case DS.APITipo.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case DS.APITipo.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null; // Inicializo respuesta
                apiResponse = await client.SendAsync(message); // Envio mensaje
                var apiContent = await apiResponse.Content.ReadAsStringAsync(); //Recibo respuesta
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent); //Convierto respuesta
                return APIResponse; // Retorno resultado

            }
            catch (Exception ex)
            {// En caso de errores
                var dto = new APIResponse
                {
                    ErrorsMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsExitoso = false
                };
                // Conversion de respuesta
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;


            }
        }
    }
}
