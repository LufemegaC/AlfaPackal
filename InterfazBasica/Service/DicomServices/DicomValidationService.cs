using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Models.Dtos.MainEntities;
using InterfazBasica_DCStore.Service.IService.Dicom;


namespace InterfazBasica_DCStore.Service.DicomServices;
public class DicomValidationService : IDicomValidationService
{
    public DicomStatus IsValidDicomFile(DicomFile dicomFile)
    {
        // Verificación de nulidad del archivo DICOM y sus componentes esenciales
        if (dicomFile == null || dicomFile.Dataset == null || dicomFile.FileMetaInfo == null)
            return DicomStatus.InvalidObjectInstance;

        // Identificador de estudio
        if (!dicomFile.Dataset.Contains(DicomTag.StudyInstanceUID))
            return DicomStatus.InvalidAttributeValue;

        // Obtención de UIDs
        string sopClassUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPClassUID, null);
        string instanceUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, null);
        string studyUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.StudyInstanceUID, null);
        string seriesUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SeriesInstanceUID, null);

        // Verificación de la presencia y validez de UIDs
        if (string.IsNullOrWhiteSpace(sopClassUID) || !DicomUID.IsValidUid(sopClassUID))
            return DicomStatus.NoSuchSOPClass;
        if (!DicomUID.IsValidUid(instanceUID) || !DicomUID.IsValidUid(studyUID) || !DicomUID.IsValidUid(seriesUID))
            return DicomStatus.InvalidAttributeValue;

        // Validación de que sea un SOP Class de imagenología
        DicomUID sopClassDicomUID = DicomUID.Parse(sopClassUID);
        if (!sopClassDicomUID.IsImageStorage)
            return DicomStatus.SOPClassNotSupported;

        // Verificar la coherencia entre el Media Storage SOP Class UID y SOP Class UID dentro del Dataset
        DicomUID mediaStorageSOPClassUID = dicomFile.FileMetaInfo.MediaStorageSOPClassUID;
        if (mediaStorageSOPClassUID != sopClassDicomUID)
            return DicomStatus.StorageDataSetDoesNotMatchSOPClassError;

        // Paso las validaciones
        return DicomStatus.Success;
    }

    public DicomStatus ValidateCreateDtos(MetadataDto mainEntitiesDto)
    {
        DicomStatus status = DicomStatus.Success;
        try
        {
            ushort studyCode = ValidateMainEntitiesCreate(mainEntitiesDto);
            if (studyCode != DicomStatus.Success.Code)
            {
                status = DicomStatus.Lookup(studyCode);
            }
        }
        catch (Exception ex)
        {
            status = new DicomStatus("A90D", DicomState.Failure, "Internal server error.", ex.Message);
        }
        return status;
    }

    // *** Intenal methods ***//

    internal ushort ValidateMainEntitiesCreate(MetadataDto mainEntitiesDto)
    {
        // Validación de PatientName
        if (string.IsNullOrWhiteSpace(mainEntitiesDto.PatientName))
            return 0xA901; // Code for "Invalid or missing patient name"

        // Validación de StudyInstanceUID
        if (string.IsNullOrWhiteSpace(mainEntitiesDto.StudyInstanceUID))
            return 0xA902; // Code for "Invalid or missing StudyInstanceUID"

        // Validación de Modality
        if (string.IsNullOrWhiteSpace(mainEntitiesDto.Modality))
            return 0xA903; // Code for "Invalid or missing Modality"

        // Validación de StudyDate
        if (mainEntitiesDto.StudyDate > DateTime.Now)
            return 0xA904; // Code for "Study date is in the future"

        // Validación de AccessionNumber
        if (mainEntitiesDto.AccessionNumber != null && mainEntitiesDto.AccessionNumber.Length > 64)
            return 0xA905; // Code for "AccessionNumber exceeds the maximum length"

        // Validación de SeriesInstanceUID
        if (string.IsNullOrWhiteSpace(mainEntitiesDto.SeriesInstanceUID))
            return 0xA906; // Code for "Invalid or missing SeriesInstanceUID"

        // Validación de Series Modality
        if (string.IsNullOrWhiteSpace(mainEntitiesDto.Modality))
            return 0xA907; // Code for "Invalid or missing Modality"

        // Validación de SeriesDateTime
        //if (mainEntitiesDto.SeriesDateTime.HasValue && mainEntitiesDto.SeriesDateTime > DateTime.Now)
        //    return 0xA908; // Code for "Series date is in the future"

        // Validación de SOPInstanceUID
        if (string.IsNullOrWhiteSpace(mainEntitiesDto.SOPInstanceUID))
            return 0xA909; // code for "Invalid or missing SOPInstanceUID"

        // Validación de ImageNumber
        if (mainEntitiesDto.InstanceNumber <= 0)
            return 0xA90A; // code for "ImageNumber is invalid"

        // Validación de Rows y Columns
        if (mainEntitiesDto.Rows <= 0 || mainEntitiesDto.Columns <= 0)
            return 0xA90C; // code for "Invalid dimension information"

        // Validación de PhotometricInterpretation
        if (string.IsNullOrWhiteSpace(mainEntitiesDto.PhotometricInterpretation))
            return 0xA90B; // warning code for "Photometric interpretation information is missing"

        // Si todas las validaciones pasan
        return DicomStatus.Success.Code;
    }
}

