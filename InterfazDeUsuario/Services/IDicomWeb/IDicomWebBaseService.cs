using InterfazDeUsuario.Models;
using Newtonsoft.Json.Linq;

namespace InterfazDeUsuario.Services.DicomWeb
{
    public interface IDicomWebBaseService
    {
        /// <summary>
        /// Sends an asynchronous API request and returns the response.
        /// </summary>
        /// <typeparam name="T">The type of the expected response.</typeparam>
        /// <param name="apiRequest">The API request object containing the request details.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the response of type 
        /// <typeparamref name="T"/>.
        /// </returns>
        Task<JArray> DicomSendAsync (APIRequest apiRequest);
    }
}
