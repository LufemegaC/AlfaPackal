using DicomProcessingService.Dtos;

namespace DicomProcessingService.Services.Interfaces
{
    public interface IBaseService
    {
        public APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
