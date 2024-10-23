using Api_PACsServer.Models;
using Api_PACsServer.Services.IService.Dicom;
using FellowOakDicom;

namespace Api_PACsServer.Services
{
    public class AzureDicomFileService : IDicomFileService
    {
        public Task<OperationResult> StoreDicomFileAsync(DicomFile dicomFile, string studyInstanceUID, string seriesInstanceUID, string sopInstanceUID)
        {
            throw new NotImplementedException();
        }
    }
}
