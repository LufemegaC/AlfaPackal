using Api_PACsServer.Modelos.Dto.Vistas;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repositorio.IRepositorio.FrontEndData;
using Api_PACsServer.Services.IService.FrontendData;
using Api_PACsServer.Services.IService.Pacs;
using Api_PACsServer.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Services.FrontendData
{
    public class StudiesPatientService : IStudiesPatientService
    {
        private readonly IRepoStudyPatientOverview _repoStudyPatient;
        private readonly IPacienteService _pacienteService;
        private readonly IEstudioService _estudioService;
        public StudiesPatientService(IPacienteService pacienteService, IEstudioService estudioService,
                                    IRepoStudyPatientOverview repoStudyPatient)
        {
            _pacienteService = pacienteService;
            _estudioService = estudioService;
            _repoStudyPatient = repoStudyPatient;
        }


        public PagedList<StudyListDto> GetMainStudiesList(ParametrosPag parametros)
        {
            try
            {
                var estudios = _repoStudyPatient.GetMainStudiesList(parametros.InstitutionId).ToList();

                var studyListDtos = estudios.Select( e => new StudyListDto
                {
                    Estudio = _estudioService.MapToDto(e),
                    Paciente = _pacienteService.MapToDto(e.Paciente),
                    DescPatientSex = e.Paciente.PatientSex == "F" ? "Femenino" : (e.Paciente.PatientSex == "M" ? "Masculino" : ""),
                    DescModality = ConverterHelp.GetDescModality(e.Modality ?? ""),
                    DescBodyPart = ConverterHelp.GetDescBodyPart(e.BodyPartExamined)
                });

                return PagedList<StudyListDto>.ToPagedList(studyListDtos.AsQueryable(), parametros.PageNumber, parametros.PageSize);
            }
            catch (Exception ex)
            {
                // Manejo de excepción general
                throw;
            }
        }

        public Task<MainEntitiesValues> ValidarExistenciaDeEntidades(MainEntitiesValues mainEntitiesValues)
        {

        }
    }
}
