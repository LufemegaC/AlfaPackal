using AlfaPackalApi.Repositorio.IRepositorio;
using Api_PACsServer.Modelos;
using Api_PACsServer.Services.IService;

namespace Api_PACsServer.Services
{
    public class ValidationService : IValidationService
    {

        private readonly IPacienteRepositorio _pacienteRepo;
        private readonly IEstudioRepositorio _estudioRepo;
        private readonly ISerieRepositorio _serieRepo;
        private readonly IImagenRepositorio _imagenRepo;

        public ValidationService(IPacienteRepositorio pacienteRepo, IEstudioRepositorio estudioRepo,
                                 ISerieRepositorio serieRepo, IImagenRepositorio imagenRepo)
        {
            _pacienteRepo = pacienteRepo;
            _estudioRepo = estudioRepo;
            _serieRepo = serieRepo;
            _imagenRepo = imagenRepo;
        }


        public async Task<MainEntitiesValues> ValidarExistenciaDeEntidades(MainEntitiesValues validationEntities)
        {
            try
            {
                //Valido datos esenciales
                if (String.IsNullOrEmpty(validationEntities.SOPInstanceUID) || (String.IsNullOrEmpty(validationEntities.StudyInstanceUID)) || (String.IsNullOrEmpty(validationEntities.SeriesInstanceUID)))
                    return validationEntities;

                // RETIRED
                // Validacion de entidad del paciente
                //if (!String.IsNullOrEmpty(validationEntities.PatientID) && !String.IsNullOrEmpty(validationEntities.IssuerOfPatientID))
                //{
                //    var patient = await _pacienteRepo.GetByMetadata(validationEntities.PatientID, validationEntities.IssuerOfPatientID);
                //    if (patient != null)
                //    {
                //        validationEntities.GeneratedPatientID = patient.GeneratedPatientID;
                //        validationEntities.PACS_PatientID = patient.PACS_PatientID;
                //        validationEntities.ExistPatient = true;
                //    }
                //}
                
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
    }
}
