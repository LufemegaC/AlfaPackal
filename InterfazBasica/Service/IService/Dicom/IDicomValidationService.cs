using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Models;
using InterfazBasica_DCStore.Models.Dtos.MainEntities;

namespace InterfazBasica_DCStore.Service.IService.Dicom
{
    public interface IDicomValidationService
    {
        DicomStatus IsValidDicomFile(DicomFile dicomFile);

        DicomStatus ValidateCreateDtos(MainEntitiesCreateDto mainEntitiesDto);
        
    }
}
