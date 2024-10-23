using Api_PACsServer.Models;
using FellowOakDicom;

namespace Api_PACsServer.Services.IService.Dicom
{
    public interface IDicomFileService
    {
        Task<OperationResult> StoreDicomFileAsync(DicomFile dicomFile, string studyInstanceUID, string seriesInstanceUID, string sopInstanceUID);
    }
}
