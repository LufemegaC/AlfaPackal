using InterfazBasica_DCStore.Models.Dicom.Web;

namespace InterfazBasica_DCStore.Service.IService.Dicom
{
    public interface IDicomWebService
    {
        /// <summary>
        /// Registers the main entities in the PACS system, including Study, Series, and Instance.
        /// </summary>
        /// <param name="content">The data transfer object containing the CreateDto models for each main entity (Study, Series, and Instance).</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains an API response indicating the success or failure of the registration process.</returns>
        Task<StowRsResponse> RegisterInstances(MultipartFormDataContent content);
    }
}
