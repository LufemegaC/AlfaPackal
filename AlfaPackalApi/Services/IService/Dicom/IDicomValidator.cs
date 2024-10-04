using FellowOakDicom;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Services.IService.Dicom
{
    public interface IDicomValidator
    {
        ValidationResult ValidateAttributesBySOPClassUID(DicomDataset dataset);
    }
}
