using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica.Models.Pacs;
using InterfazBasica_DCStore.Models;
using System.Text.RegularExpressions;

namespace InterfazBasica_DCStore.Service.IDicomService
{
    public interface IDicomValidationService
    // Interfaz para la validacion de archivos Dicom 
    {
        DicomStatus IsValidDicomFile(DicomFile dicomFile);
        Task<MainEntitiesValues> ValidateEntities(MainEntitiesValues mainEntitiesValues);
    }
}
