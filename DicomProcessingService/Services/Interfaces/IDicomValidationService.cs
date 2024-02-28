using FellowOakDicom;
using System.Collections.Generic;

namespace DicomProcessingService.Services.Interfaces
{
    public interface IDicomValidationService
    {
        bool IsValidDicomUid(string uid);
        string GenerateUid();
        bool ValidateDataset(DicomDataset dataset);
        bool ValidateAgainstStandard(DicomDataset dataset);
        bool ValidateImageIntegrity(DicomFile dicomFile);
    }
}
