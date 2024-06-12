using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Modelos.Dto.Pacs;
using AlfaPackalApi.Repositorio.IRepositorio;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Dto.Vistas;
using Api_PACsServer.Services.IService;
using Api_PACsServer.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Services
{
    public class GeneralAPIService : IGeneralAPIService
    {

        private readonly IPacienteRepositorio _pacienteRepo;
        private readonly IEstudioRepositorio _estudioRepo;
        private readonly ISerieRepositorio _serieRepo;
        private readonly IImagenRepositorio _imagenRepo;
        private readonly ApplicationDbContext _db;

        public GeneralAPIService(IPacienteRepositorio pacienteRepo, IEstudioRepositorio estudioRepo,
                                 ISerieRepositorio serieRepo, IImagenRepositorio imagenRepo,
                                 ApplicationDbContext db)
        {
            _pacienteRepo = pacienteRepo;
            _estudioRepo = estudioRepo;
            _serieRepo = serieRepo;
            _imagenRepo = imagenRepo;
            _db = db;
        }


        public async Task<MainEntitiesValues> ValidarExistenciaDeEntidades(MainEntitiesValues validationEntities)
        {
            try
            {
                //Valido datos esenciales
                if (String.IsNullOrEmpty(validationEntities.SOPInstanceUID) || (String.IsNullOrEmpty(validationEntities.StudyInstanceUID)) || (String.IsNullOrEmpty(validationEntities.SeriesInstanceUID)))
                    return validationEntities;

                // validacion de entidad de estudio
                var study = await _estudioRepo.GetByInstanceUID(validationEntities.StudyInstanceUID);
                if (study != null)
                {
                    validationEntities.PACS_EstudioID = study.PACS_EstudioID;
                    validationEntities.ExistStudy = true;
                    var patient = await _pacienteRepo.ObtenerPorID(v => v.PACS_PatientID == study.PACS_PatientID);
                    validationEntities.PACS_PatientID = patient.PACS_PatientID;
                    validationEntities.GeneratedPatientID = patient.GeneratedPatientID;
                    validationEntities.ExistPatient = true;
                }
                // validacion de serie
                var serie = await _serieRepo.GetByInstanceUID(validationEntities.SeriesInstanceUID);
                if (serie != null)
                {
                    validationEntities.PACS_SerieID = serie.PACS_SerieID;
                    validationEntities.ExistSerie = true;
                }
                // Validacion de imagen
                var image = await _imagenRepo.GetBySOPInstanceUID(validationEntities.SOPInstanceUID);
                if (image != null)
                {
                    validationEntities.PACS_ImagenID = image.PACS_ImagenID;
                    validationEntities.ExistImage = true;
                }
                return validationEntities;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //Listado amain

        public async Task<List<EstudioConPacienteDto>> ObtenerEstudiosConPaciente()
        {
            List<EstudioConPacienteDto> result = null;
            // Utiliza la funcionalidad de LINQ y Entity Framework para realizar la consulta
            try
            {
                result = await _db.Estudios
                .Include(e => e.Paciente) // Asume que existe una relación entre Estudios y Pacientes
                .OrderByDescending(e => e.StudyDate) 
                .Select(e => new EstudioConPacienteDto
                {
                    PatientName = e.Paciente.PatientName.Replace("^^^", "^").Replace("^", " ").Replace("  ", " "),
                    PatientID = e.Paciente.PatientID,
                    PatientSex = e.Paciente.PatientSex,
                    DescPatientSex = e.Paciente.PatientSex == "F" ? "Femenino" : (e.Paciente.PatientSex == "M" ? "Masculino" : ""),
                    PatientAge = e.Paciente.PatientAge,
                    StudyDescription = e.StudyDescription,
                    Modality = e.Modality,
                    DescModality = ConverterHelp.GetDescModality(e.Modality ?? ""),
                    StudyDate = e.StudyDate,
                    BodyPartExamined = e.BodyPartExamined,
                    DescBodyPart = ConverterHelp.GetDescBodyPart(e.BodyPartExamined),
                    InstitutionName = e.InstitutionName
                })
                .Take(15)
                .ToListAsync(); 
            }
            catch (SqlException sqlEx)
            {
                throw; // Re-throw the exception to let the caller know about it
            }
            catch (Exception ex)
            {
                throw; // Re-throw the exception to let the caller know about it
            }

            return result;
        }

        public async Task<List<StudyListVM>> GetStudyList(int institutionId)
        {
            List<StudyListVM> result = null;
            try
            {
                result = await _db.Estudios
                .Include(e => e.Paciente) // Asume que existe una relación entre Estudios y Pacientes
                .Where(e => e.InstitutionId == institutionId) // Filtro por institución
                .OrderByDescending(e => e.StudyDate)
                .Select(e => new StudyListVM
                {
                    Estudio = new EstudioDto
                    {
                        StudyInstanceUID = e.StudyInstanceUID,
                        StudyComments = e.StudyComments,
                        Modality = e.Modality,
                        StudyDescription = e.StudyDescription,
                        StudyDate = e.StudyDate,
                        BodyPartExamined = e.BodyPartExamined,
                        AccessionNumber = e.AccessionNumber,
                        PerformingPhysicianName = e.PerformingPhysicianName,
                        OperatorName = e.OperatorName,
                        ExposureTime = e.ExposureTime,
                        KVP = e.KVP,
                        NumberOfFrames = e.NumberOfFrames
                    },
                    Paciente = new PacienteDto
                    {
                        PatientID = e.Paciente.PatientID,
                        PatientName = e.Paciente.PatientName.Replace("^^^", "^").Replace("^", " ").Replace("  ", " "),
                        PatientAge = e.Paciente.PatientAge,
                        PatientSex = e.Paciente.PatientSex,
                        PatientWeight = e.Paciente.PatientWeight,
                        PatientBirthDate = e.Paciente.PatientBirthDate,
                        IssuerOfPatientID = e.Paciente.IssuerOfPatientID
                    },
                    DescPatientSex = e.Paciente.PatientSex == "F" ? "Femenino" : (e.Paciente.PatientSex == "M" ? "Masculino" : ""),
                    DescModality = ConverterHelp.GetDescModality(e.Modality ?? ""),
                    DescBodyPart = ConverterHelp.GetDescBodyPart(e.BodyPartExamined)
                })
                .Take(15)
                .ToListAsync();
            }
            catch (SqlException sqlEx)
            {
                throw; // Re-throw the exception to let the caller know about it
            }
            catch (Exception ex)
            {
                throw; // Re-throw the exception to let the caller know about it
            }
            return result;
        }

        // Adaptacion de llamados para C-FIND
        public async Task<List<string>> FindPatientFiles(string patientName, string patientId)
        {
            try
            {
                var matchedPatientIds = await _pacienteRepo.FindByCriteria(patientName, patientId);
                var matchedStudies = await _estudioRepo.FindByPatientIds(matchedPatientIds);

                var dicomFileUrls = matchedStudies
                                    .Where(s => !string.IsNullOrEmpty(s.DicomFileLocation))
                                    .Select(s => s.DicomFileLocation)
                                    .ToList();

                return dicomFileUrls;
            }
            catch (Exception ex)
            {
                // Manejo adecuado de la excepción
                return null;
            }
        }

        public async Task<List<string>> FindStudyFiles(string patientName, string patientId, string accessionNbr, string studyUID)
        {

            try
            {
                // Realiza la consulta utilizando los modelos de EF Core directamente.
                // Esta consulta asume que tienes relaciones configuradas entre las entidades y que
                // el nombre de las propiedades en los modelos reflejan esos nombres de relaciones.
                var query = _db.Estudios.AsQueryable();

                // Filtra por PatientName si es proporcionado
                if (!string.IsNullOrEmpty(patientName))
                {
                    query = query.Where(e => e.Paciente.PatientName.Contains(patientName));
                }

                if (!string.IsNullOrEmpty(patientId))
                {
                    query = query.Where(e => e.Paciente.PatientID.Contains(patientId));
                }

                if (!string.IsNullOrEmpty(accessionNbr))
                {
                    query = query.Where(e => e.AccessionNumber.Contains(accessionNbr));
                }

                if (!string.IsNullOrEmpty(studyUID))
                {
                    query = query.Where(e => e.StudyInstanceUID.Contains(studyUID));
                }

                // Selecciona la ubicación del archivo DICOM y realiza la paginación
                var dicomFileUrls = await query
                    .OrderByDescending(e => e.StudyDate)
                    .Take(10)
                    .Select(e => e.DicomFileLocation)
                    .ToListAsync();

                return dicomFileUrls.Where(url => !string.IsNullOrEmpty(url)).ToList();
            }
            catch (SqlException sqlEx)
            {
                // Manejo adecuado de la excepción de SQL
                throw;
            }
            catch (Exception ex)
            {
                // Manejo adecuado de excepciones generales
                // Considera loguear la excepción
                throw;
            }

        }
        public async Task<List<string>> FindSeriesFiles(string PatientName, string PatientId, string AccessionNbr, string StudyUID, string SeriesUID, string Modality)
        { 
            




            return null;
        
        
        
        }
        public async Task<List<string>> FindFilesByUID(string PatientId, string StudyUID, string SeriesUID)
        { return null; }
        
    }
}
