using AutoMapper;
using FellowOakDicom;

namespace DicomProcessingService.Services.Interfaces
{
    public interface IDicomDecompositionService
    {
        Dictionary<DicomTag, object> ExtractMetadata(DicomDataset dicomDataset);
        IEnumerable<byte[]> ExtractImageFrames(DicomFile dicomFile);
        string GetDicomElementAsString(DicomDataset dicomDataset, DicomTag dicomTag);
        DateTime? GetStudyDateTime(DicomDataset dicomDataset);
        void ConvertDicomToFile(DicomFile dicomFile, string outputPath, DicomTransferSyntax transferSyntax);
        void UpdatePatientData(DicomFile dicomFile, string patientName, string patientId);
        void AnonymizeDicomFile(DicomFile dicomFile);
    }
}
