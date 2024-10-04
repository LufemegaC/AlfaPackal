using FellowOakDicom;

namespace Api_PACsServer.Services.IService.Dicom
{
    public interface IDicomDataCoercion
    {
        void Anonymize(DicomDataset dataset);
        void CorrectData(DicomDataset dataset);
    }
}
