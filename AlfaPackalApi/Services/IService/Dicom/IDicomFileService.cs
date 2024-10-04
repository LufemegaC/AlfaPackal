using Api_PACsServer.Models;

namespace Api_PACsServer.Services.IService.Dicom
{
    public interface IDicomFileService
    {
        Task<OperationResult> StoreDicomFileAsync(IFormFile dicomFile, string studyInstanceUID, string seriesInstanceUID, string sopInstanceUID);
    }
}
