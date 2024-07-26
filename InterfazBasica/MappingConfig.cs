using AutoMapper;
using FellowOakDicom;
using InterfazBasica.Models.Pacs;
using InterfazBasica_DCStore.Models;
using InterfazBasica_DCStore.Utilities;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Utileria;
using static InterfazBasica_DCStore.Utilities.DicomUtilities;
using static Utileria.DicomValues;

namespace InterfazBasica_DCStore
{
    public class MappingConfig : Profile
    {
        private string _rootPath;

        public MappingConfig(IConfiguration configuration) {
            // 
            _rootPath = configuration.GetValue<string>("DicomSettings:StoragePath");


            //Paciente 
            CreateMap<PacienteDto, PacienteCreateDto>().ReverseMap();
            CreateMap<PacienteDto, PacienteUpdateDto>().ReverseMap();
            //Estudio
            CreateMap<EstudioDto, EstudioCreateDto>().ReverseMap();
            CreateMap<EstudioDto, EstudioUpdateDto>().ReverseMap();
            //Serie
            CreateMap<SerieDto, SerieCreateDto>().ReverseMap();
            CreateMap<SerieDto, SerieUpdateDto>().ReverseMap();
            //Imagen
            CreateMap<ImagenDto, ImagenCreateDto>().ReverseMap();
            CreateMap<ImagenDto, ImagenUpdateDto>().ReverseMap();

            //** Mapeo de Diccionario de metadatos a entidades de creacion Dto
            //Paciente
            CreateMap<Dictionary<DicomTag, object>, PacienteCreateDto>()
                .ConvertUsing(src => MapDictionaryToPacienteCreateDto(src));
            //Estudio
            CreateMap<Dictionary<DicomTag, object>, EstudioCreateDto>()
                .ConvertUsing(src => MapDictionaryToEstudioCreateDto(src));
            //Serie
            CreateMap<Dictionary<DicomTag, object>, SerieCreateDto>()
                .ConvertUsing(src => MapDictionaryToSerieCreateDto(src));
            //Imagen
            CreateMap<Dictionary<DicomTag, object>, ImagenCreateDto>()
                .ConvertUsing(src => MapDictionaryToImagenCreateDto(src));


        }

        private static PacienteCreateDto MapDictionaryToPacienteCreateDto(Dictionary<DicomTag, object> dicomTagDictionary)
        // Mapeo de Diccionario de metadatos a entidad de creacion de paciente.
        {
            var pacienteDto = new PacienteCreateDto
            {
                // Generados por el sistema
                GeneratedPatientID = PatientIDGenerator(),
                //IssuerOfPatientID = DicomUtilities.IssuerOfPatientIDValue,
                // Valores de metadatos
                PatientID = dicomTagDictionary.TryGetValue(DicomTag.PatientID, out object patientIdValue) ? patientIdValue as string : string.Empty,
                IssuerOfPatientID = dicomTagDictionary.TryGetValue(DicomTag.IssuerOfPatientID, out object issuerOfPatientIdValue) ? issuerOfPatientIdValue as string : string.Empty,
                PatientName = dicomTagDictionary.TryGetValue(DicomTag.PatientName, out object patientNameValue) ? patientNameValue as string : string.Empty,
                PatientAge = dicomTagDictionary.TryGetValue(DicomTag.PatientAge, out object patientAgeValue) ? patientAgeValue as string : string.Empty,
                PatientSex = dicomTagDictionary.TryGetValue(DicomTag.PatientSex, out object patientSexValue) ? patientSexValue as string : string.Empty,
                PatientWeight = dicomTagDictionary.TryGetValue(DicomTag.PatientWeight, out object patientWeightValue) ? patientWeightValue as string : string.Empty,
                
            };

            // Asumimos que la fecha de nacimiento viene en formato string y necesita ser convertida a DateTime
            if (dicomTagDictionary.TryGetValue(DicomTag.PatientBirthDate, out object patientBirthDateValue) && DateTime.TryParseExact(patientBirthDateValue as string, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime patientBirthDate))
            {
                pacienteDto.PatientBirthDate = patientBirthDate;
            }

            return pacienteDto;
        }

        private static EstudioCreateDto MapDictionaryToEstudioCreateDto(Dictionary<DicomTag, object> dicomTagDictionary)
        //Mapeo de Diccionario de metadatos a entidad de creacion de estudio.
        {
            
            var estudioDto = new EstudioCreateDto
            {
                StudyInstanceUID = dicomTagDictionary.TryGetValue(DicomTag.StudyInstanceUID, out object studyInstanceUidValue) ? studyInstanceUidValue as string : string.Empty,
                //PACS_PatientID = null, // Este valor parece que debe ser establecido de otra manera o mantenerse como nulo
                Modality = dicomTagDictionary.TryGetValue(DicomTag.Modality, out object modalityValue) ? modalityValue as string : string.Empty,
                StudyDescription = dicomTagDictionary.TryGetValue(DicomTag.StudyDescription, out object studyDescriptionValue) ? studyDescriptionValue as string : string.Empty,
                AccessionNumber = dicomTagDictionary.TryGetValue(DicomTag.AccessionNumber, out object accessionNumberValue) ? accessionNumberValue as string : GenerateAccessionNumber(),
                InstitutionName = dicomTagDictionary.TryGetValue(DicomTag.InstitutionName, out object institutionNameValue) ? institutionNameValue as string : string.Empty,
                //DicomFileLocation = fullPath, // Location of dicom file
                PerformingPhysicianName = dicomTagDictionary.TryGetValue(DicomTag.PerformingPhysicianName, out object performingPhysicianNameValue) ? performingPhysicianNameValue as string : string.Empty,
                OperatorName = dicomTagDictionary.TryGetValue(DicomTag.OperatorsName, out object operatorNameValue) ? operatorNameValue as string : string.Empty,
                ExposureTime = dicomTagDictionary.TryGetValue(DicomTag.ExposureTime, out object exposureTimeValue) ? exposureTimeValue as string : string.Empty,
                KVP = dicomTagDictionary.TryGetValue(DicomTag.KVP, out object kvpValue) ? kvpValue as string : string.Empty,
                NumberOfFrames = dicomTagDictionary.TryGetValue(DicomTag.NumberOfFrames, out object numberOfFramesValue) ? Convert.ToInt32(numberOfFramesValue) : (int?)null,
                BodyPartExamined = dicomTagDictionary.TryGetValue(DicomTag.BodyPartExamined, out object bodyPartExaminedValue) ? bodyPartExaminedValue as string : string.Empty,
            };

            if (dicomTagDictionary.TryGetValue(DicomTag.StudyDate, out object studyDateValue) && DateTime.TryParseExact(studyDateValue as string, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime studyDate))
            {
                estudioDto.StudyDate = studyDate;
            }

            return estudioDto;
        }

        private static SerieCreateDto MapDictionaryToSerieCreateDto(Dictionary<DicomTag, object> dicomTagDictionary)
        {
            try
            {
                var serieDto = new SerieCreateDto
                {
                    StudyInstanceUID =  dicomTagDictionary.TryGetValue(DicomTag.StudyInstanceUID, out object estudioIdValue) ? estudioIdValue as string : string.Empty,
                    SeriesDescription = dicomTagDictionary.TryGetValue(DicomTag.SeriesDescription, out object seriesDescriptionValue) ? seriesDescriptionValue as string : string.Empty,
                    SeriesInstanceUID = dicomTagDictionary.TryGetValue(DicomTag.SeriesInstanceUID, out object seriesInstanceUIDValue) ? seriesInstanceUIDValue as string : string.Empty,
                    SeriesNumber      = dicomTagDictionary.TryGetValue(DicomTag.SeriesNumber, out object seriesNumberValue) ? Convert.ToInt32(seriesNumberValue) : (int?)null,
                    Modality          = dicomTagDictionary.TryGetValue(DicomTag.Modality, out object modalityValue) ? modalityValue as string : string.Empty,
                    BodyPartExamined  = dicomTagDictionary.TryGetValue(DicomTag.BodyPartExamined, out object bodyPartExaminedValue) ? bodyPartExaminedValue as string : string.Empty,
                    PatientPosition   = dicomTagDictionary.TryGetValue(DicomTag.PatientPosition, out object patientPositionValue) ? patientPositionValue as string : string.Empty,
                };
                // Serie date
                if (dicomTagDictionary.TryGetValue(DicomTag.SeriesDate, out object seriesDateTimeValue) && DateTime.TryParse(seriesDateTimeValue as string, out DateTime seriesDateTime))
                {
                    serieDto.SeriesDateTime = seriesDateTime;
                }
                return serieDto;
            }
            catch (Exception ex)
            {
                var msj = Convert.ToString(ex.Message);
                return null; 
            }
            
            
        }

        private static ImagenCreateDto MapDictionaryToImagenCreateDto(Dictionary<DicomTag, object> dicomTagDictionary)
        {
            var studyUID = dicomTagDictionary.TryGetValue(DicomTag.StudyInstanceUID, out object estudioIdValue) ? estudioIdValue as string : string.Empty;
            var serieUID = dicomTagDictionary.TryGetValue(DicomTag.SeriesInstanceUID, out object seriesInstanceUID) ? seriesInstanceUID as string : string.Empty;
            var instanceUID = dicomTagDictionary.TryGetValue(DicomTag.SOPInstanceUID, out object sopInstanceUIDValue) ? sopInstanceUIDValue as string : string.Empty;
            var imagenDto = new ImagenCreateDto
            {
                SOPInstanceUID = studyUID,
                SeriesInstanceUID = serieUID,
                StudyInstanceUID = instanceUID,
                ImageNumber = dicomTagDictionary.TryGetValue(DicomTag.InstanceNumber, out object imageNumberValue) ? Convert.ToInt32(imageNumberValue) : 0,
                ImageComments = dicomTagDictionary.TryGetValue(DicomTag.ImageComments, out object imageCommentsValue) ? imageCommentsValue as string : string.Empty,
                //RETIRED ImageLocation = dicomTagDictionary.TryGetValue(DicomTag.ImageLocation, out object imageLocationValue) ? imageLocationValue as string : string.Empty,
                //TransferSyntaxUID = dicomTagDictionary.TryGetValue(DicomTag.TransferSyntaxUID, out object transferSyntaxUIDValue) ? transferSyntaxUIDValue as string : string.Empty,
                PhotometricInterpretation = dicomTagDictionary.TryGetValue(DicomTag.PhotometricInterpretation, out object photometricInterpretationValue) ? photometricInterpretationValue as string : string.Empty,
                Rows = dicomTagDictionary.TryGetValue(DicomTag.Rows, out object rowsValue) ? Convert.ToInt32(rowsValue) : 0,
                Columns = dicomTagDictionary.TryGetValue(DicomTag.Columns, out object columnsValue) ? Convert.ToInt32(columnsValue) : 0,
                PixelSpacing = dicomTagDictionary.TryGetValue(DicomTag.PixelSpacing, out object pixelSpacingValue) ? pixelSpacingValue as string : string.Empty,
                //12/07/24 LFMG: Ruta de almacenamiento.
                ImageLocation = RootFileDicomConstructor(studyUID, serieUID, instanceUID)
            };

        return imagenDto;
        }

        internal string RootFileDicomConstructor(string StudyUID, string SerieUID, string InstanceUID)
        {
            // Construye la ruta completa usando StudyInstanceUID y SeriesInstanceUID.
            var fullPath = Path.Combine(_rootPath, StudyUID, SerieUID);
            // Verifica si la ruta del directorio existe, si no, la crea.
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            // Completa la ruta del archivo añadiendo el SOPInstanceUID y la extensión .dcm.
            var filePath = Path.Combine(fullPath, InstanceUID + ".dcm");
            return filePath;
        }


    }
}
