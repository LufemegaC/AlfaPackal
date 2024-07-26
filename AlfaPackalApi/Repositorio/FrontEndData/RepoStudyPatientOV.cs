using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Modelos.Dto.Vistas;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Repositorio.IRepositorio.FrontEndData;
using Api_PACsServer.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Api_PACsServer.Repositorio.FrontEndData
{
    public class RepoStudyPatientOV : IRepoStudyPatientOverview
    {
        private readonly ApplicationDbContext _db;
        public RepoStudyPatientOV(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<Estudio> GetMainStudiesList(int institutionId)
        {
            return _db.Estudios
            .Include(e => e.Paciente)
            .Where(e => e.InstitutionId == institutionId)
            .OrderByDescending(e => e.StudyDate);



            //IQueryable<StudyListDto> listVMs = null;
            //try
            //{
            //    listVMs = _db.Estudios
            //    .Include(e => e.Paciente) // Incluye la entidad Paciente si hay una relación establecida
            //    .Where(e => e.InstitutionId == parametros.InstitutionId) // Filtro por InstitutionId
            //    .OrderByDescending(e => e.StudyDate)
            //    .Select(e => new StudyListDto
            //    {
            //        Estudio = _mapper.Map<EstudioDto>(e.Estudio),
            //        Paciente = new PacienteDto
            //        {
            //            PatientID = e.Paciente.PatientID,
            //            PatientName = e.Paciente.PatientName.Replace("^^^", "^").Replace("^", " ").Replace("  ", " "),
            //            PatientAge = e.Paciente.PatientAge,
            //            PatientSex = e.Paciente.PatientSex,
            //            PatientWeight = e.Paciente.PatientWeight,
            //            PatientBirthDate = e.Paciente.PatientBirthDate,
            //            IssuerOfPatientID = e.Paciente.IssuerOfPatientID
            //        },
            //        DescPatientSex = e.Paciente.PatientSex == "F" ? "Femenino" : (e.Paciente.PatientSex == "M" ? "Masculino" : ""),
            //        DescModality = ConverterHelp.GetDescModality(e.Modality ?? ""),
            //        DescBodyPart = ConverterHelp.GetDescBodyPart(e.BodyPartExamined)
            //    });
            //    return PagedList<StudyListDto>.ToPagedList(listVMs, parametros.PageNumber, parametros.PageSize);

            //}
            //catch (SqlException sqlEx)
            //{
            //    throw; // Re-throw the exception to let the caller know about it
            //}
            //catch (Exception ex)
            //{
            //    throw; // Re-throw the exception to let the caller know about it
            //}
        }
    }
}
