using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.AccessControl;
using AutoMapper;
using Api_PACsServer.Models.Dto.Series;
using Api_PACsServer.Models.Dto.DicomServer;
using Api_PACsServer.Models.Dto.AuthDtos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Models.Dto.Instances;
using Api_PACsServer.Utilities;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models.OHIFVisor;

namespace AlfaPackalApi
{
    public class MappingConfig : Profile
    {
        // Mapeo de Data Transfer Objects
        public MappingConfig() {
            // Studies
            CreateMap<Study, StudyCreateDto>().ReverseMap();
            CreateMap<StudyCreateDto, MainEntitiesCreateDto>().ReverseMap();
            CreateMap<Study, StudyDto>()
             .ForMember(dest => dest.DescModality, opt => opt.MapFrom(src => ConverterHelp.GetDescModality(src.Modality)))
             .ForMember(dest => dest.DescBodyPartE, opt => opt.MapFrom(src => ConverterHelp.GetDescBodyPart(src.BodyPartExamined)))
             .ReverseMap();
            CreateMap<StudyDetails, StudyDetailsUpdateDto>().ReverseMap();
            CreateMap<StudyDetails, StudyDetailsCreateDto>().ReverseMap();
            CreateMap<Study, OHIFStudy>()
             .ConvertUsing(src => MapStudyToOHIFStudy(src));
            // Serie
            CreateMap<Serie, SerieCreateDto>().ReverseMap();
            CreateMap<SerieCreateDto, MainEntitiesCreateDto>().ReverseMap();
            CreateMap<SerieDetails, SerieDetailsCreateDto>().ReverseMap();
            CreateMap<SerieDetails, SerieDetailsUpdateDto>().ReverseMap();
            CreateMap<Serie, OHIFSerie>()
             .ConvertUsing(src => MapSerieToOHIFSeries(src));

            // Instances
            CreateMap<Instance, InstanceCreateDto>().ReverseMap();
            CreateMap<InstanceCreateDto, MainEntitiesCreateDto>().ReverseMap();
            CreateMap<Instance, InstanceDto>().ReverseMap();
            CreateMap<InstanceDetails, InstanceDetailsCreateDto>().ReverseMap();
            CreateMap<Instance, OHIFInstance>()
             .ConvertUsing(src => MapInstanceToOHIFInstance(src));
            // Users
            CreateMap<SystemUser, UserDto>().ReverseMap();
            CreateMap<RegisterRequestDto, SystemUser>()
                .ConvertUsing(src => MapRegisterRequestToSystemUser(src));
            // Servers
            CreateMap<LocalDicomServer,LocalDicomServerDto>().ReverseMap();
        }

        private static SystemUser MapRegisterRequestToSystemUser(RegisterRequestDto registerRequest)
        {
            return new SystemUser
            {
                UserName = registerRequest.UserName,
                FullName = registerRequest.FullName,
                Email = registerRequest.Email,
                NormalizedEmail = registerRequest.Email.ToUpper(),
                InstitutionId = registerRequest.InstitutionId,
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
    }
}
