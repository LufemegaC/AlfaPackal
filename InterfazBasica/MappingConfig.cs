using AutoMapper;
using FellowOakDicom;
using InterfazBasica_DCStore.Models.Dtos.MainEntities;
using System.Globalization;
using static InterfazBasica_DCStore.Utilities.DicomUtility;

namespace InterfazBasica_DCStore
{
    public class MappingConfig : Profile
    {

        public MappingConfig() {

            //// Main entities
            //CreateMap<StudyDto, StudyCreateDto>().ReverseMap();
            //CreateMap<SerieDto, SerieCreateDto>().ReverseMap();
            //CreateMap<InstanceDto, InstanceCreateDto>().ReverseMap();

            //** Mapeo de Diccionario de metadatos a entidades de creacion Dto
            // Mapeo de Diccionario de metadatos a MainEntitiesCreateDto
            CreateMap<Dictionary<DicomTag, object>, MainEntitiesCreateDto>()
                .ConvertUsing(src => MapDictionaryToMainEntitiesCreateDto(src));

        }

        private static MainEntitiesCreateDto MapDictionaryToMainEntitiesCreateDto(Dictionary<DicomTag, object> dicomTagDictionary)
        {
            var mainEntitiesDto = new MainEntitiesCreateDto
            {
                // Mapeo de propiedades comunes en Estudio, Serie e Instancia
                SOPClassUID = dicomTagDictionary.TryGetValue(DicomTag.SOPClassUID, out object sopClassUidValue) ? sopClassUidValue as string : string.Empty,
                SOPInstanceUID = dicomTagDictionary.TryGetValue(DicomTag.SOPInstanceUID, out object sopInstanceUidValue) ? sopInstanceUidValue as string : string.Empty,
                SeriesInstanceUID = dicomTagDictionary.TryGetValue(DicomTag.SeriesInstanceUID, out object seriesInstanceUidValue) ? seriesInstanceUidValue as string : string.Empty,
                StudyInstanceUID = dicomTagDictionary.TryGetValue(DicomTag.StudyInstanceUID, out object studyInstanceUidValue) ? studyInstanceUidValue as string : string.Empty,
                TransferSyntaxUID = dicomTagDictionary.TryGetValue(DicomTag.TransferSyntaxUID, out object transferSyntaxValue) ? transferSyntaxValue as string : string.Empty,

                // Información de la Instancia
                ImageComments = dicomTagDictionary.TryGetValue(DicomTag.ImageComments, out object imageCommentsValue) ? imageCommentsValue as string : string.Empty,
                InstanceNumber = dicomTagDictionary.TryGetValue(DicomTag.InstanceNumber, out object imageNumberValue) ? Convert.ToInt32(imageNumberValue) : 0,
                PhotometricInterpretation = dicomTagDictionary.TryGetValue(DicomTag.PhotometricInterpretation, out object photometricInterpretationValue) ? photometricInterpretationValue as string : string.Empty,
                Rows = dicomTagDictionary.TryGetValue(DicomTag.Rows, out object rowsValue) ? Convert.ToInt32(rowsValue) : 0,
                Columns = dicomTagDictionary.TryGetValue(DicomTag.Columns, out object columnsValue) ? Convert.ToInt32(columnsValue) : 0,
                PixelSpacing = dicomTagDictionary.TryGetValue(DicomTag.PixelSpacing, out object pixelSpacingValue) ? pixelSpacingValue as string : string.Empty,
                NumberOfFrames = dicomTagDictionary.TryGetValue(DicomTag.NumberOfFrames, out object numberOfFramesValue) ? Convert.ToInt32(numberOfFramesValue) : (int?)null,
                ImagePositionPatient = dicomTagDictionary.TryGetValue(DicomTag.ImagePositionPatient, out object imagePositionValue)? imagePositionValue as string: string.Empty,
                ImageOrientationPatient = dicomTagDictionary.TryGetValue(DicomTag.ImageOrientationPatient, out object imageOrientationValue)? imageOrientationValue as string: string.Empty,

                // Información de la Serie
                SeriesDescription = dicomTagDictionary.TryGetValue(DicomTag.SeriesDescription, out object seriesDescriptionValue) ? seriesDescriptionValue as string : string.Empty,
                SeriesNumber = dicomTagDictionary.TryGetValue(DicomTag.SeriesNumber, out object seriesNumberValue) ? Convert.ToInt32(seriesNumberValue) : (int?)null,
                Modality = dicomTagDictionary.TryGetValue(DicomTag.Modality, out object modalityValue) ? modalityValue as string : string.Empty,
                PatientPosition = dicomTagDictionary.TryGetValue(DicomTag.PatientPosition, out object patientPositionValue) ? patientPositionValue as string : string.Empty,

                // Información del Estudio
                StudyDescription = dicomTagDictionary.TryGetValue(DicomTag.StudyDescription, out object studyDescriptionValue) ? studyDescriptionValue as string : string.Empty,
                AccessionNumber = dicomTagDictionary.TryGetValue(DicomTag.AccessionNumber, out object accessionNumberValue) ? accessionNumberValue as string : string.Empty,
                InstitutionName = dicomTagDictionary.TryGetValue(DicomTag.InstitutionName, out object institutionNameValue) ? institutionNameValue as string : string.Empty,
                BodyPartExamined = dicomTagDictionary.TryGetValue(DicomTag.BodyPartExamined, out object bodyPartExaminedValue) ? bodyPartExaminedValue as string : string.Empty,
                StudyTime = dicomTagDictionary.TryGetValue(DicomTag.StudyTime, out object studyTimeValue)? TimeSpan.TryParseExact(studyTimeValue as string, "hhmmss", null, out var parsedStudyTime)
                            ? parsedStudyTime: TimeSpan.Zero: TimeSpan.Zero,
                PatientBirthDate = dicomTagDictionary.TryGetValue(DicomTag.PatientBirthDate, out object birthDateValue)? DateTime.TryParseExact(birthDateValue as string, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var parsedBirthDate)
                            ? parsedBirthDate: DateTime.MinValue: DateTime.MinValue,

                // Información del Paciente
                PatientName = dicomTagDictionary.TryGetValue(DicomTag.PatientName, out object patientNameValue) ? patientNameValue as string : string.Empty,
                PatientAge = dicomTagDictionary.TryGetValue(DicomTag.PatientAge, out object patientAgeValue) ? patientAgeValue as string : string.Empty,
                PatientSex = dicomTagDictionary.TryGetValue(DicomTag.PatientSex, out object patientSexValue) ? patientSexValue as string : string.Empty,
                PatientWeight = dicomTagDictionary.TryGetValue(DicomTag.PatientWeight, out object patientWeightValue) ? patientWeightValue as string : string.Empty,
                IssuerOfPatientID = dicomTagDictionary.TryGetValue(DicomTag.IssuerOfPatientID, out object issuerOfPatientIdValue) ? issuerOfPatientIdValue as string : string.Empty
            };

            // Convertir StudyDate y SeriesDateTime si están disponibles
            if (dicomTagDictionary.TryGetValue(DicomTag.StudyDate, out object studyDateValue) && DateTime.TryParseExact(studyDateValue as string, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime studyDate))
            {
                mainEntitiesDto.StudyDate = studyDate;
            }

            if (dicomTagDictionary.TryGetValue(DicomTag.SeriesDate, out object seriesDateValue) && DateTime.TryParseExact(seriesDateValue as string, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime seriesDateTime))
            {
                mainEntitiesDto.SeriesDateTime = seriesDateTime;
            }

            return mainEntitiesDto;
        }
    }
}
