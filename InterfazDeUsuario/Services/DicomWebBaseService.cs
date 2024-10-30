using InterfazDeUsuario.Models;
using InterfazDeUsuario.Services.DicomWeb;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using static InterfazDeUsuario.Utility.LocalUtility;

namespace InterfazDeUsuario.Services
{
    public class DicomWebBaseService : IDicomWebBaseService
    {
        public IHttpClientFactory _httpClient { get; set; }
        public DicomWebBaseService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Decision for 1.1: APIRequest class will be sufficient for QIDO-RS.
        /// This is because the APIRequest class already covers all necessary properties like URL, APIType, and optional request data and token.
        /// </summary>

        // Solution for 1.2 and 2: Creating a version of DicomSendAsync for QIDO-RS

        public async Task<JArray> DicomSendAsync(APIRequest apiRequest)
        {
            try
            {
                if (apiRequest.APIType != APIType.GET)
                {
                    throw new InvalidOperationException("Only GET requests are allowed for this entity.");
                }

                var client = _httpClient.CreateClient("dicomAPI");
                HttpRequestMessage message = new HttpRequestMessage
                {
                    RequestUri = new Uri(apiRequest.Url),
                    Method = HttpMethod.Get
                };

                // Add authorization token if present
                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                // Send the request and read response
                HttpResponseMessage apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                // Deserialize to JArray
                return JArray.Parse(apiContent);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new InvalidOperationException($"An error occurred during the DICOM request: {ex.Message}", ex);
            }
        }

    }
}
