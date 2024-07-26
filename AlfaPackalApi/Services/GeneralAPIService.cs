using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos.Dto.Pacs;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Dto.Vistas;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Services.IService;
using Api_PACsServer.Services.IService.Pacs;
using Api_PACsServer.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Services
{
    public class GeneralAPIService : IGeneralAPIService
    {

        //private readonly IPacienteRepositorio _pacienteRepo;
        //private readonly IEstudioRepositorio _estudioRepo;
        //private readonly ISerieRepositorio _serieRepo;
        //private readonly IImagenRepositorio _imagenRepo;

        private readonly IPacienteService _pacienteService;
        private readonly IEstudioService _estudioService;
        private readonly ISerieService _serieService;
        private readonly IImagenService _imagenService;
        private readonly ApplicationDbContext _db;
        //internal DbSet<EstudioConPacienteDto> DbSet;

        public GeneralAPIService(IPacienteService pacienteService, IEstudioService estudioService,
                                 ISerieService serieService, IImagenService imagenService,
                                 ApplicationDbContext db)
        {
            _pacienteService = pacienteService;
            _estudioService = estudioService;
            _serieService = serieService;
            _imagenService = imagenService;
            _db = db;
            //this.DbSet = _db.Set<EstudioConPacienteDto>();
        }


        //public async Task<MainEntitiesValues> ValidarExistenciaDeEntidades(MainEntitiesValues validationEntities)
        //{
        //    try
        //    {
        //        //Valido datos esenciales
        //        if (string.IsNullOrEmpty(validationEntities.SOPInstanceUID) || (string.IsNullOrEmpty(validationEntities.StudyInstanceUID)) || (string.IsNullOrEmpty(validationEntities.SeriesInstanceUID)))
        //            return validationEntities;

        //        // validacion de entidad de estudio
        //        var response = await _estudioService.GetByUID(validationEntities.StudyInstanceUID);
        //        var study = (EstudioDto)response.Resultado;
        //        if (study != null)
        //        {
        //            validationEntities.PACS_EstudioID = study.PACS_EstudioID;
        //            validationEntities.ExistStudy = true;
        //            var patient = await _pacienteRepo.Obtener(v => v.PACS_PatientID == study.PACS_PatientID);
        //            validationEntities.PACS_PatientID = patient.PACS_PatientID;
        //            validationEntities.GeneratedPatientID = patient.GeneratedPatientID;
        //            validationEntities.ExistPatient = true;
        //        }
        //        // validacion de serie
        //        var serie = await _serieService.GetByInstanceUID(validationEntities.SeriesInstanceUID);
        //        if (serie != null)
        //        {
        //            validationEntities.PACS_SerieID = serie.PACS_SerieID;
        //            validationEntities.ExistSerie = true;
        //        }
        //        // Validacion de imagen
        //        var image = await _imagenService.GetByInstanceUID(validationEntities.SOPInstanceUID);
        //        if (image != null)
        //        {
        //            validationEntities.PACS_ImagenID = image.PACS_ImagenID;
        //            validationEntities.ExistImage = true;
        //        }
        //        return validationEntities;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        //Listado amain

        //public async Task<List<EstudioConPacienteDto>> ObtenerEstudiosConPaciente()
        //{
        //    List<EstudioConPacienteDto> result = null;
        //    // Utiliza la funcionalidad de LINQ y Entity Framework para realizar la consulta
        //    try
        //    {
        //        result = await _db.Estudios
        //        .Include(e => e.Paciente) // Asume que existe una relación entre Estudios y Pacientes
        //        .OrderByDescending(e => e.StudyDate) 
        //        .Select(e => new EstudioConPacienteDto
        //        {
        //            PatientName = e.Paciente.PatientName.Replace("^^^", "^").Replace("^", " ").Replace("  ", " "),
        //            PatientID = e.Paciente.PatientID,
        //            PatientSex = e.Paciente.PatientSex,
        //            DescPatientSex = e.Paciente.PatientSex == "F" ? "Femenino" : (e.Paciente.PatientSex == "M" ? "Masculino" : ""),
        //            PatientAge = e.Paciente.PatientAge,
        //            StudyDescription = e.StudyDescription,
        //            Modality = e.Modality,
        //            DescModality = ConverterHelp.GetDescModality(e.Modality ?? ""),
        //            StudyDate = e.StudyDate,
        //            BodyPartExamined = e.BodyPartExamined,
        //            DescBodyPart = ConverterHelp.GetDescBodyPart(e.BodyPartExamined),
        //            InstitutionName = e.InstitutionName
        //        })
        //        .Take(15)
        //        .ToListAsync(); 
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        throw; // Re-throw the exception to let the caller know about it
        //    }
        //    catch (Exception ex)
        //    {
        //        throw; // Re-throw the exception to let the caller know about it
        //    }

        //    return result;
        //}

        //public async Task<List<StudyListDto>> GetStudyList(int institutionId)
        //{
        //    List<StudyListDto> result = null;
        //    try
        //    {
        //        result = await _db.Estudios
        //        .Include(e => e.Paciente) // Asume que existe una relación entre Estudios y Pacientes
        //        .Where(e => e.InstitutionId == institutionId) // Filtro por institución
        //        .OrderByDescending(e => e.StudyDate)
        //        .Select(e => new StudyListDto
        //        {
        //            Estudio = new EstudioDto
        //            {
        //                StudyInstanceUID = e.StudyInstanceUID,
        //                StudyComments = e.StudyComments,
        //                Modality = e.Modality,
        //                StudyDescription = e.StudyDescription,
        //                StudyDate = e.StudyDate,
        //                BodyPartExamined = e.BodyPartExamined,
        //                AccessionNumber = e.AccessionNumber,
        //                PerformingPhysicianName = e.PerformingPhysicianName,
        //                OperatorName = e.OperatorName,
        //                ExposureTime = e.ExposureTime,
        //                KVP = e.KVP,
        //                //NumberOfFrames = e.NumberOfFrames
        //            },
        //            Paciente = new PacienteDto
        //            {
        //                PatientID = e.Paciente.PatientID,
        //                PatientName = e.Paciente.PatientName.Replace("^^^", "^").Replace("^", " ").Replace("  ", " "),
        //                PatientAge = e.Paciente.PatientAge,
        //                PatientSex = e.Paciente.PatientSex,
        //                PatientWeight = e.Paciente.PatientWeight,
        //                PatientBirthDate = e.Paciente.PatientBirthDate,
        //                IssuerOfPatientID = e.Paciente.IssuerOfPatientID
        //            },
        //            DescPatientSex = e.Paciente.PatientSex == "F" ? "Femenino" : (e.Paciente.PatientSex == "M" ? "Masculino" : ""),
        //            DescModality = ConverterHelp.GetDescModality(e.Modality ?? ""),
        //            DescBodyPart = ConverterHelp.GetDescBodyPart(e.BodyPartExamined)
        //        })
        //        .Take(15)
        //        .ToListAsync();
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        throw; // Re-throw the exception to let the caller know about it
        //    }
        //    catch (Exception ex)
        //    {
        //        throw; // Re-throw the exception to let the caller know about it
        //    }
        //    return result;
        //}


        //public PagedList<StudyListDto> ListaEstudiosPaginado(ParametrosPag parametros)
        //{
        //    IQueryable<StudyListDto> listVMs = null;
        //    try
        //    {
        //        listVMs = _db.Estudios
        //        .Include(e => e.Paciente) // Incluye la entidad Paciente si hay una relación establecida
        //        .Where(e => e.InstitutionId == parametros.InstitutionId) // Filtro por InstitutionId
        //        .OrderByDescending(e => e.StudyDate)
        //        .Select(e => new StudyListDto
        //        {
        //            Estudio = _mapper.Map<EstudioDto>(e.Estudio),
        //            Paciente = new PacienteDto
        //            {
        //                PatientID = e.Paciente.PatientID,
        //                PatientName = e.Paciente.PatientName.Replace("^^^", "^").Replace("^", " ").Replace("  ", " "),
        //                PatientAge = e.Paciente.PatientAge,
        //                PatientSex = e.Paciente.PatientSex,
        //                PatientWeight = e.Paciente.PatientWeight,
        //                PatientBirthDate = e.Paciente.PatientBirthDate,
        //                IssuerOfPatientID = e.Paciente.IssuerOfPatientID
        //            },
        //            DescPatientSex = e.Paciente.PatientSex == "F" ? "Femenino" : (e.Paciente.PatientSex == "M" ? "Masculino" : ""),
        //            DescModality = ConverterHelp.GetDescModality(e.Modality ?? ""),
        //            DescBodyPart = ConverterHelp.GetDescBodyPart(e.BodyPartExamined)
        //        });
        //        return PagedList<StudyListDto>.ToPagedList(listVMs, parametros.PageNumber, parametros.PageSize);

        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        throw; // Re-throw the exception to let the caller know about it
        //    }
        //    catch (Exception ex)
        //    {
        //        throw; // Re-throw the exception to let the caller know about it
        //    }

        //}
    }
}
