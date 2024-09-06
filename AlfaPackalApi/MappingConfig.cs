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
            // Serie
            CreateMap<Serie, SerieCreateDto>().ReverseMap();
            CreateMap<SerieCreateDto, MainEntitiesCreateDto>().ReverseMap();
            CreateMap<SerieDetails, SerieDetailsCreateDto>().ReverseMap();
            CreateMap<SerieDetails, SerieDetailsUpdateDto>().ReverseMap();
            // Instances
            CreateMap<Instance, InstanceCreateDto>().ReverseMap();
            CreateMap<InstanceCreateDto, MainEntitiesCreateDto>().ReverseMap();
            CreateMap<Instance, InstanceDto>().ReverseMap();
            CreateMap<InstanceDetails, InstanceDetailsCreateDto>().ReverseMap();
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
    }
}
