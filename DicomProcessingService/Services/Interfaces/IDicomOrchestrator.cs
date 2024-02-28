using FellowOakDicom;
using FellowOakDicom.Network;

namespace DicomProcessingService.Services.Interfaces
{
    public interface IDicomOrchestrator
    {
        Task HandleCStoreRequestAsync(DicomCStoreRequest cStoreRequest);
    }


}
