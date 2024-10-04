using Api_PACsServer.Models;
using Api_PACsServer.Services.IService.Dicom;

namespace Api_PACsServer.Services
{
    public class AzureDicomFileService : IDicomFileService
    {
        public Task<OperationResult> StoreDicomFileAsync(IFormFile dicomFile, string studyInstanceUID, string seriesInstanceUID, string sopInstanceUID)
        {
            throw new NotImplementedException();
        }
    }
}
