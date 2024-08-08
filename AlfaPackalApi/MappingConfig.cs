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

namespace AlfaPackalApi
{
    public class MappingConfig : Profile
    {
        // Mapeo de Data Transfer Objects
        public MappingConfig() {
            // Studies
            CreateMap<Study, StudyCreateDto>().ReverseMap();
            CreateMap<Study, StudyDto>().ReverseMap(); 
            CreateMap<StudyLoad, StudyLoadUpdateDto>().ReverseMap();
            // Serie
            CreateMap<Serie, SerieCreateDto>().ReverseMap();
            CreateMap<Serie, SerieUpdateDto>().ReverseMap();
            CreateMap<SerieLoad, SerieUpdateDto>().ReverseMap(); 
            // Instances
            CreateMap<Instance, InstanceCreateDto>().ReverseMap();
            CreateMap<Instance, InstanceDto>().ReverseMap();
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
