using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Models;

namespace InterfazBasica_DCStore.Service.IDicomService
{
    public interface IDicomOrchestrator
    {
        Task<DicomStatus> StoreDicomData (DicomFile dicomFile);

        Task<DicomStatus> StoreDicomFile(DicomFile dicomFile);
    }
}
