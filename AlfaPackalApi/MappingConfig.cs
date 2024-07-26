using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto.Pacs;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Modelos.Cargas;
using Api_PACsServer.Modelos.Dto;
using AutoMapper;
using FellowOakDicom;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
            //** Emtidades de carga **//
            // Estudio Carga
            CreateMap<EstudioCarga, Estudio>().ReverseMap();//Create
            CreateMap<SerieCarga, Serie>().ReverseMap();//Create
            CreateMap<ImagenCarga, ImagenCreateDto>().ReverseMap();//Create

            // Login
            CreateMap<UsuarioSistema, UsuarioDto>().ReverseMap();
            // Server 
            CreateMap<DicomServer, DicomServerDto > ().ReverseMap();
        }

    }
}
