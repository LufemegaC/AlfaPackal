using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto.Pacs;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Modelos.Dto;
using AutoMapper;

namespace AlfaPackalApi
{
    public class MappingConfig : Profile
    {
        // Mapeo de Data Transfer Objects
        public MappingConfig() {
            //Paciente
            CreateMap<Paciente, PacienteCreateDto>().ReverseMap(); //Create
            CreateMap<Paciente, PacienteUpdateDto>().ReverseMap(); //Update
            CreateMap<Paciente, PacienteDto>().ReverseMap(); // Other
            // Estudio
            CreateMap<Estudio, EstudioCreateDto>().ReverseMap();//Create
            CreateMap<Estudio, EstudioUpdateDto>().ReverseMap();//Update
            CreateMap<Estudio, EstudioDto>().ReverseMap(); // Other
            // Serie
            CreateMap<Serie, SerieCreateDto>().ReverseMap();//Create
            CreateMap<Serie, SerieUpdateDto>().ReverseMap();//Update
            CreateMap<Serie, SerieDto>().ReverseMap(); // Other
            //Imagen
            CreateMap<Imagen, ImagenCreateDto>().ReverseMap();//Create
            CreateMap<Imagen, ImagenUpdateDto>().ReverseMap();//Update
            CreateMap<Imagen, ImagenDto>().ReverseMap(); // Other
            // Login
            CreateMap<UsuarioSistema, UsuarioDto>().ReverseMap();          
        }
    }
}
