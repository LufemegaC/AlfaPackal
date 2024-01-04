using AutoMapper;
using InterfazBasica.Models.Pacs;

namespace InterfazBasica_DCStore
{
    public class MappingConfig: Profile
    {
        public MappingConfig() {
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

        }
    }
}
