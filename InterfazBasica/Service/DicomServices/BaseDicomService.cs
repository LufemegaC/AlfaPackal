using InterfazBasica.Models;
using InterfazBasica_DCStore.Models.Dicom.Web;
using InterfazBasica_DCStore.Service.IService.Dicom;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static InterfazBasica_DCStore.Utilities.LocalUtility;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class BaseDicomService : IBaseDicomService
    {
        public DicomAPIRequest responseModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }

        public BaseDicomService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            _httpClient = httpClient;
        }


        public async Task<T> DicomSendAsync<T>(DicomAPIRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("dicomAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.RequestUri = new Uri(apiRequest.Url);

                // Set the HTTP method
                switch (apiRequest.APIType)
                {
                    case APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                // Add authorization token if present
                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                // Handle multipart content for STOW-RS
                if (apiRequest.RequestData is MultipartContent multipartContent)
                {
                    // Set the Content-Type for the multipart content
                    multipartContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/related; type=\"application/dicom\"");
                    message.Content = multipartContent;
                }
                else
                {
                    throw new InvalidOperationException("RequestData must be of type MultipartContent for DICOM operations.");
                }

                // Send the request
                HttpResponseMessage apiResponse = await client.SendAsync(message);

                // Read the response content
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                // Deserialize the response
                var response = JsonConvert.DeserializeObject<T>(apiContent);

                return response;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                var errorResponse = new APIResponse
                {
                    ErrorsMessages = new List<string> { ex.Message },
                    IsSuccessful = false
                };
                var errorContent = JsonConvert.SerializeObject(errorResponse);
                var response = JsonConvert.DeserializeObject<T>(errorContent);
                return response;
            }
        }
    }
}
