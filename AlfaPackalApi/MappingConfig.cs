using AutoMapper;
using Api_PACsServer.Models.Dto.Series;
using Api_PACsServer.Models.Dto.DicomServer;
using Api_PACsServer.Models.Dto.AuthDtos;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Models.Dto.Instances;
using Api_PACsServer.Models.OHIFVisor;
using Api_PACsServer.Models.DicomSupport;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Models;
using Api_PACsServer.Models.AccessControl;
using static Api_PACsServer.Utilities.QueryOperators;
using Api_PACsServer.Models.Dto.DicomWeb;
using FellowOakDicom;
using System.Globalization;
using System.Data;

namespace Api_PACsServer
{
    public class MappingConfig : Profile
    {
        // Mapeo de Data Transfer Objects
        public MappingConfig()
        {
            // Dentro del constructor de MappingConfig:
            CreateMap<DicomDataset, MetadataDto>()
                .ConvertUsing(src => MapDicomDatasetToMetadataDto(src));

            // Studies
            CreateMap<Study, StudyCreateDto>().ReverseMap();
            CreateMap<StudyCreateDto, MetadataDto>().ReverseMap();
            CreateMap<Study, StudyDto>().ReverseMap();
            CreateMap<StudyCreateDto, StudyModalityCreateDto>().ReverseMap();
            CreateMap<StudyModality, StudyModalityCreateDto>().ReverseMap();
            //.ForMember(dest => dest.DescModality, opt => opt.MapFrom(src => ConverterHelp.GetDescModality(src.Modality)))
            //.ForMember(dest => dest.DescBodyPartE, opt => opt.MapFrom(src => ConverterHelp.GetDescBodyPart(src.BodyPartExamined)))

            CreateMap<StudyDetails, StudyDetailsUpdateDto>().ReverseMap();
            CreateMap<StudyDetails, StudyDetailsCreateDto>().ReverseMap();
            CreateMap<Study, OHIFStudy>().ConvertUsing(src => MapStudyToOHIFStudy(src));
            CreateMap<(Study, StudyDetails), StudyDto>()
             .ConvertUsing(src => MapStudyAndDetailsToDto(src.Item1, src.Item2));
            // Serie
            CreateMap<Serie, SerieCreateDto>().ReverseMap();
            CreateMap<SerieCreateDto, MetadataDto>().ReverseMap();
            CreateMap<SerieDetails, SerieDetailsCreateDto>().ReverseMap();
            CreateMap<SerieDetails, SerieDetailsUpdateDto>().ReverseMap();
            CreateMap<Serie, OHIFSerie>()
             .ConvertUsing(src => MapSerieToOHIFSeries(src));

            // Instances
            CreateMap<Instance, InstanceCreateDto>().ReverseMap();
            CreateMap<InstanceCreateDto, MetadataDto>().ReverseMap();
            CreateMap<Instance, InstanceDto>().ReverseMap();
            CreateMap<InstanceDetails, InstanceDetailsCreateDto>().ReverseMap();
            CreateMap<Instance, OHIFInstance>()
             .ConvertUsing(src => MapInstanceToOHIFInstance(src));
            // Users
            CreateMap<SystemUser, UserDto>().ReverseMap();
            CreateMap<RegisterRequestDto, SystemUser>()
                .ConvertUsing(src => MapRegisterRequestToSystemUser(src));
            // Servers
            CreateMap<LocalDicomServer, LocalDicomServerDto>().ReverseMap();
        }

        private static SystemUser MapRegisterRequestToSystemUser(RegisterRequestDto registerRequest)
        {
            return new SystemUser
            {
                UserName = registerRequest.UserName,
                FullName = registerRequest.FullName,
                Email = registerRequest.Email,
                NormalizedEmail = registerRequest.Email.ToUpper(),
                //InstitutionId = registerRequest.InstitutionId,
                Rol = registerRequest.Rol,
            };
        }

        private static OHIFStudy MapStudyToOHIFStudy(Study study)
        {
            return new OHIFStudy
            {
                StudyInstanceUID = study.StudyInstanceUID,
                StudyDate = study.StudyDate.ToString("yyyyMMdd"),  // Formato DICOM para la fecha
                StudyTime = study.StudyTime?.ToString(@"hhmmss") ?? string.Empty,     // Formato DICOM para la hora
                AccessionNumber = study.StudyDescription,          // Mapeo directo de StudyDescription
                //PatientID = study.IssuerOfPatientID,               // Mapea IssuerOfPatientID como PatientID
                PatientName = study.PatientName,                   // Mapeo directo de PatientName
                PatientBirthDate = study.PatientBirthDate?.ToString("yyyyMMdd") ?? string.Empty,  // Convertir DateTime a string
                StudyDescription = study.StudyDescription          // Mapeo directo de StudyDescription
            };
        }

        private static OHIFSerie MapSerieToOHIFSeries(Serie serie)
        {
            return new OHIFSerie
            {
                SeriesInstanceUID = serie.SeriesInstanceUID,
                Modality = serie.Modality,  // Mapeo directo de Modality
                SeriesNumber = serie.SeriesNumber.ToString(),  // Convertir a string
                SeriesDescription = serie.SeriesDescription,  // Mapeo directo de SeriesDescription
                Instances = null  // Para el mapeo de instancias, esto se puede ajustar más adelante
            };
        }

        private static OHIFInstance MapInstanceToOHIFInstance(Instance instance)
        {
            return new OHIFInstance
            {
                SOPInstanceUID = instance.SOPInstanceUID,  // Mapeo directo
                InstanceNumber = instance.InstanceNumber.ToString(),  // Convertir int a string
                TransferSyntaxUID = instance.TransferSyntaxUID,  // Mapeo directo
                ImagePositionPatient = instance.ImagePositionPatient,  // Depende de InstanceDetails, si existe
                ImageOrientationPatient = instance.ImageOrientationPatient,  // Depende de InstanceDetails, si existe
                PixelSpacing = instance.PixelSpacing  // Mapeo directo, ya que PixelSpacing es opcional
            };
        }

        private static StudyDto MapStudyAndDetailsToDto(Study study, StudyDetails studyDetails)
        {
            return new StudyDto
            {
                StudyInstanceUID = study.StudyInstanceUID,
                StudyID = study.StudyID,
                StudyDescription = study.StudyDescription,
                StudyDate = study.StudyDate,
                StudyTime = study.StudyTime,
                InstitutionName = study.InstitutionName,
                ReferringPhysicianName = study.ReferringPhysicianName,
                PatientName = study.PatientName,
                PatientAge = study.PatientAge,
                PatientSex = study.PatientSex,
                PatientWeight = study.PatientWeight,
                IssuerOfPatientID = study.IssuerOfPatientID,
                NumberOfStudyRelatedInstances = studyDetails.NumberOfStudyRelatedInstances,
                NumberOfStudyRelatedSeries = studyDetails.NumberOfStudyRelatedSeries,
                TotalFileSizeMB = studyDetails.TotalFileSizeMB,
                ModalitiesInStudy = study.ModalitiesInStudy != null
                                    ? string.Join(", ", study.ModalitiesInStudy.Select(m => m.Modality))
                                    : string.Empty
            };
        }


        // MAPEOS DE Dtos con Dicom
        // Diccionario para atributos del estudio: StudyColumnTypeMapping
        public static readonly Dictionary<string, QueryParameterType> StudyColumnTypeMapping = new Dictionary<string, QueryParameterType>
        {
            { "studydate", QueryParameterType.DateTime },          // (0008,0020) Study Date - VR=DA
            { "studytime", QueryParameterType.String },            // (0008,0030) Study Time - VR=TM
            { "accessionnumber", QueryParameterType.String },      // (0008,0050) Accession Number - VR=SH
            { "modality", QueryParameterType.String },             // (0008,0060) Modality - VR=CS
            { "modalitiesinstudy", QueryParameterType.String },    // (0008,0061) Modalities in Study - VR=CS
            { "referringphysicianname", QueryParameterType.String },// (0008,0090) Referring Physician's Name - VR=PN
            { "studydescription", QueryParameterType.String },     // (0008,1030) Study Description - VR=LO
            { "seriesdescription", QueryParameterType.String },    // (0008,103E) Series Description - VR=LO
            { "patientname", QueryParameterType.String },          // (0010,0010) Patient's Name - VR=PN
            { "patientid", QueryParameterType.String },            // (0010,0020) Patient ID - VR=LO
            { "patientbirthdate", QueryParameterType.DateTime },   // (0010,0030) Patient's Birth Date - VR=DA
            { "patientsex", QueryParameterType.String },           // (0010,0040) Patient's Sex - VR=CS
            { "studystatusid", QueryParameterType.String },        // Custom field (could vary based on your implementation)
            { "numberofstudyrelatedseries", QueryParameterType.Int },  // Derived number of series related to a study
            { "numberofstudyrelatedinstances", QueryParameterType.Int },// Derived number of instances related to a study
            { "studyinstanceuid", QueryParameterType.String }      // (0020,000D) Study Instance UID - VR=UI
        
        };


        // Diccionario para atributos de control: ControlColumnTypeMapping
        public static readonly Dictionary<string, QueryParameterType> ControlColumnTypeMapping = new Dictionary<string, QueryParameterType>
        {
            { "limit", QueryParameterType.Int },
            { "orderby", QueryParameterType.String },
            { "offset", QueryParameterType.Int },
            { "includefields", QueryParameterType.String },
            { "format", QueryParameterType.String },
            { "page", QueryParameterType.Int },
            { "pagesize", QueryParameterType.Int }
        };

        // Método de mapeo fuera del constructor:
        private static MetadataDto MapDicomDatasetToMetadataDto(DicomDataset dataset)
        {
            return new MetadataDto
            {
                // -- Main information -- //
                SOPClassUID = dataset.GetSingleValueOrDefault(DicomTag.SOPClassUID, string.Empty),
                SOPInstanceUID = dataset.GetSingleValueOrDefault(DicomTag.SOPInstanceUID, string.Empty),
                SeriesInstanceUID = dataset.GetSingleValueOrDefault(DicomTag.SeriesInstanceUID, string.Empty),
                StudyInstanceUID = dataset.GetSingleValueOrDefault(DicomTag.StudyInstanceUID, string.Empty),
                TransferSyntaxUID = dataset.GetSingleValueOrDefault(DicomTag.TransferSyntaxUID, (string?)null),


                // -- Instance information -- //
                ImageComments = dataset.GetSingleValueOrDefault(DicomTag.ImageComments, string.Empty),
                InstanceNumber = dataset.GetSingleValueOrDefault(DicomTag.InstanceNumber, 0),
                PhotometricInterpretation = dataset.GetSingleValueOrDefault(DicomTag.PhotometricInterpretation, string.Empty),
                Rows = dataset.GetSingleValueOrDefault(DicomTag.Rows, 0),
                Columns = dataset.GetSingleValueOrDefault(DicomTag.Columns, 0),
                PixelSpacing = dataset.GetString(DicomTag.PixelSpacing),
                NumberOfFrames = dataset.GetSingleValueOrDefault<int>(DicomTag.NumberOfFrames, 0),
                ImagePositionPatient = dataset.GetString(DicomTag.ImagePositionPatient),
                ImageOrientationPatient = dataset.GetString(DicomTag.ImageOrientationPatient),
                BitsAllocated = dataset.GetSingleValueOrDefault(DicomTag.BitsAllocated, 0),
                SliceThickness = dataset.GetSingleValueOrDefault(DicomTag.SliceThickness, 0f),
                SliceLocation = dataset.GetSingleValueOrDefault(DicomTag.SliceLocation, 0f),
                InstanceCreationDate = dataset.GetString(DicomTag.InstanceCreationDate),
                InstanceCreationTime = dataset.GetString(DicomTag.InstanceCreationTime),
                ContentDate = dataset.GetString(DicomTag.ContentDate),
                ContentTime = dataset.GetString(DicomTag.ContentTime),
                AcquisitionDateTime = dataset.GetSingleValueOrDefault<DateTime>(DicomTag.AcquisitionDateTime, DateTime.MinValue),

                // -- Serie Information -- //
                SeriesDescription = dataset.GetSingleValueOrDefault(DicomTag.SeriesDescription, string.Empty),
                SeriesNumber = dataset.GetSingleValueOrDefault(DicomTag.SeriesNumber, 0),
                Modality = dataset.GetSingleValueOrDefault(DicomTag.Modality, string.Empty),
                PatientPosition = dataset.GetSingleValueOrDefault(DicomTag.PatientPosition, string.Empty),
                ProtocolName = dataset.GetSingleValueOrDefault(DicomTag.ProtocolName, string.Empty),
                SeriesDate = dataset.GetString(DicomTag.SeriesDate),
                SeriesTime = dataset.GetString(DicomTag.SeriesTime),

                // -- Study Information -- //
                //StudyComments = dataset.GetSingleValueOrDefault(DicomTag.StudyComments, string.Empty),
                StudyDescription = dataset.GetSingleValueOrDefault(DicomTag.StudyDescription, string.Empty),
                StudyDate = ParseDicomDate(dataset, DicomTag.StudyDate),
                StudyTime = ParseDicomTime(dataset, DicomTag.StudyTime),
                AccessionNumber = dataset.GetSingleValueOrDefault(DicomTag.AccessionNumber, string.Empty),
                InstitutionName = dataset.GetSingleValueOrDefault(DicomTag.InstitutionName, string.Empty),
                BodyPartExamined = dataset.GetSingleValueOrDefault(DicomTag.BodyPartExamined, string.Empty),

                // -- Patient Information -- //
                PatientName = dataset.GetSingleValueOrDefault(DicomTag.PatientName, string.Empty),
                PatientAge = dataset.GetSingleValueOrDefault(DicomTag.PatientAge, string.Empty),
                PatientSex = dataset.GetSingleValueOrDefault(DicomTag.PatientSex, string.Empty),
                PatientWeight = dataset.GetSingleValueOrDefault(DicomTag.PatientWeight, string.Empty),
                PatientBirthDate = ParseDicomDate(dataset, DicomTag.PatientBirthDate),
                IssuerOfPatientID = dataset.GetSingleValueOrDefault(DicomTag.IssuerOfPatientID, string.Empty),

                // -- Transaction -- //
                TransactionUID = dataset.GetSingleValueOrDefault(DicomTag.TransactionUID, string.Empty),
                TransactionStatus = dataset.GetSingleValueOrDefault(DicomTag.TransactionStatus, string.Empty),
                TransactionStatusComment = dataset.GetSingleValueOrDefault(DicomTag.TransactionStatusComment, string.Empty)
            };
        }

        // Métodos auxiliares para parsing
        private static DateTime ParseDicomDate(DicomDataset dataset, DicomTag tag)
        {
            return dataset.TryGetString(tag, out var dateStr) && DateTime.TryParseExact(dateStr, "yyyyMMdd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                ? date
                : DateTime.MinValue;
        }

        private static TimeSpan ParseDicomTime(DicomDataset dataset, DicomTag tag)
        {
            if (dataset.TryGetString(tag, out var timeStr))
            {
                if (TimeSpan.TryParseExact(timeStr, "hhmmss", CultureInfo.InvariantCulture, out var time))
                    return time;

                if (timeStr.Length >= 6 && int.TryParse(timeStr[..6], out var seconds))
                    return TimeSpan.FromSeconds(seconds);
            }
            return TimeSpan.Zero;
        }

    }
}
