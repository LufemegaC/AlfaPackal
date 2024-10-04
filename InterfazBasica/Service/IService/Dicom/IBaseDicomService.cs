using InterfazBasica_DCStore.Models.Dicom.Web;

namespace InterfazBasica_DCStore.Service.IService.Dicom
{
    public interface IBaseDicomService
    {
        /// <summary>
        /// Gets or sets the API response model.
        /// </summary>
        public DicomAPIRequest responseModel { get; set; }

        /// <summary>
        /// Sends an asynchronous API request and returns the response.
        /// </summary>
        /// <typeparam name="T">The type of the expected response.</typeparam>
        /// <param name="apiRequest">The API request object containing the request details.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the response of type 
        /// <typeparamref name="T"/>.
        /// </returns>
        Task<T> DicomSendAsync<T>(DicomAPIRequest apiRequest);
    }
}
