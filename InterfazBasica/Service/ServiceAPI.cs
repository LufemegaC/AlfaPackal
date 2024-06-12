using InterfazBasica.Models;
using InterfazBasica.Models.Pacs;
using InterfazBasica.Service.IService;
using InterfazBasica_DCStore.Models;
using InterfazBasica_DCStore.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Xml.Linq;
using Utileria;
using static System.Net.Mime.MediaTypeNames;

namespace InterfazBasica_DCStore.Service
{
    public class ServiceAPI : IServiceAPI
    {
        private readonly IPacienteService _pacienteService;
        private readonly IEstudioService _estudioService;
        private readonly ISerieService _serieService;
        private readonly IImageService _imagenService;
        // Contexto de Session:
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _token; //Token de autorizacion

        public ServiceAPI(IPacienteService pacienteService, IEstudioService estudioService,
            ISerieService serieService, IImageService imageService, IHttpContextAccessor httpContextAccessor)
        {
            _pacienteService = pacienteService;
            _estudioService = estudioService;
            _serieService = serieService;
            _imagenService = imageService;
            _httpContextAccessor = httpContextAccessor;
        }
        // ** METODOS DE REGISTRO DE ENTIDADES ** //
        // PACIENTE
        public async Task<APIResponse> RegistrarPaciente(PacienteCreateDto modelo)
        {
            try
            {
                // Structure valid
                var resultValid = IsValidPacienteCreate(modelo);
                if (!resultValid.Success)
                    APIResponse.NoValidEntity(resultValid.ErrorsMessages, modelo);
                // Valido si ya existe el registro
                 var response = await _pacienteService.Create<APIResponse>(modelo, Token);
                //var response2 = await _pacienteService.CreatePruebas<APIResponse>(modelo);
                return response;
            }
            catch (Exception ex)
            {
                // Manejo de errores al intentar registrar al paciente.
                return APIResponse.NoValidEntity(new List<string> { ex.Message }, modelo);
            }
        }

        // ESTUDIO
        public async Task<APIResponse> RegistrarEstudio(EstudioCreateDto modelo)
        {
            // Structure valid
            var resultValid = IsValidEstudioCreate(modelo);
            if (!resultValid.Success)
                APIResponse.NoValidEntity(resultValid.ErrorsMessages, modelo);
            // Valido si ya existe el registro
            var existStudy = await _estudioService.ExistStudyByInstanceUID<APIResponse>(modelo.StudyInstanceUID, Token);
            if (existStudy != null && existStudy.Resultado is bool && (bool)existStudy.Resultado)
                APIResponse.NoValidEntity(new List<string> { "El paciente ya está registrado con el ID proporcionado." }, modelo);
            try
            {
                var response = await _estudioService.Create<APIResponse>(modelo, Token);
                return response;
            }
            catch (Exception ex)
            {
                // Manejo de errores al intentar registrar al paciente.
                return APIResponse.NoValidEntity(new List<string> { ex.Message }, modelo);
            }
        }
        // SERIE
        public async Task<APIResponse> RegistrarSerie(SerieCreateDto modelo)
        {
            // Structure valid
            var resultValid = IsValidSerieCreate(modelo);
            if (!resultValid.Success)
                APIResponse.NoValidEntity(resultValid.ErrorsMessages, modelo);
            try
            {
                var response = await _serieService.Create<APIResponse>(modelo, Token);
                return response;
            }
            catch (Exception ex)
            {
                // Manejo de errores al intentar registrar al paciente.
                return APIResponse.NoValidEntity(new List<string> { ex.Message }, modelo);
            }
        }
        // IMAGEN
        public async Task<APIResponse> RegistrarImagen(ImagenCreateDto modelo)
        {
            // Structure valid
            var resultValid = IsValidImagenCreate(modelo);
            if (!resultValid.Success)
                APIResponse.NoValidEntity(resultValid.ErrorsMessages, modelo);
            // Proceso de registros
            try
            {
                var response = await _imagenService.Create<APIResponse>(modelo, Token);
                return response;
            }
            catch (Exception ex)
            {
                // Manejo de errores al intentar registrar al paciente.
                return APIResponse.NoValidEntity(new List<string> { ex.Message }, modelo);
            }

        }


        // ** METODOS PARA VALIDAR EXISTENCIA DE ENTIDADES ** //
        // PACIENTE
        // Paciente por name
        public async Task<int?> GetPACSIDPatientByName(string name)
        {
            var patient = await _pacienteService.GetByName<APIResponse>(name, Token);
            if (patient != null && patient.IsExitoso)
            {
                return patient.PacsResourceId;//PACS_ImageId
            }
            return null;
        }


        // Paciente por ID generada por DICOM Server
        public async Task<int?> GetPACSIDPatientByGeneratedID(string generatedID)
        {
            var patient = await _pacienteService.GetByGeneratedPatientID<APIResponse>(generatedID, Token);
            if (patient != null && patient.IsExitoso)
            {
                return patient.PacsResourceId;//PACS_ImageId
            }
            return null;
        }
        // ESTUDIO
        // Estudio por Instance UID (Metadato)
        public async Task<int?> GetPACSIDStudyByInstanceUID(string studyInstanceUID)
        {
            var estudio = await _estudioService.GetStudyByInstanceUID<APIResponse>(studyInstanceUID, Token);
            if (estudio != null && estudio.IsExitoso)
            {
                return estudio.PacsResourceId;//PACS_StudyId
            }
            return null;
        }
        // SERIE
        // Serie por Instancia UID
        public async Task<int?> GetPACSIDSeriesByInstanceUID(string serieInstanceUID)
        {
            var serie = await _serieService.GetBySeriesInstanceUID<APIResponse>(serieInstanceUID, Token);
            if (serie != null && serie.IsExitoso)
            {
                return serie.PacsResourceId;//PACS_SerieId
            }
            return null;
        }
        // IMAGE
        // Imagen por SOP Instance UID
        public async Task<int?> GetPACSIDImageBySOPInstanceUID(String sopInstanceUID)
        {
            var image = await _imagenService.GetbySOPInstanceUID<APIResponse>(sopInstanceUID, Token);
            if (image != null && image.IsExitoso)
            {
                return image.PacsResourceId;//PACS_ImageId
            }
            return null;
            // Structure valid
        }

        // *** METODOS INTERNOS DE CLASE ***//
        internal OperationResult IsValidEstudioCreate(EstudioCreateDto estudioCreate)
        // Valido los componentes princiapales de mi estudio
        {
            var operationResult = OperationResult.CreateForValidation();
            // Identificador UID del estudio
            if (string.IsNullOrWhiteSpace(estudioCreate.StudyInstanceUID))
                operationResult.AddError("StudyInstanceUID no puede ser vacio.", null);
            // Modalidad de estudio
            if (string.IsNullOrWhiteSpace(estudioCreate.Modality))
                operationResult.AddError("Modality no puede ser vacío.", null);
            // Asegúrate de que la fecha del estudio no sea futura.
            if (estudioCreate.StudyDate > DateTime.Now)
            {
                string fechaEstudio = estudioCreate.StudyDate.ToString("o");
                operationResult.AddError("La fecha de estudio es mayor a la fecha actual.", fechaEstudio);
            }
            // Verifica que el AccessionNumber no exceda la longitud máxima.
            if (estudioCreate.AccessionNumber.Length > 64)
                operationResult.AddError("La longitud de AccessionNumber no es valida.", estudioCreate.AccessionNumber);
            return operationResult;
        }

        internal OperationResult IsValidImagenCreate(ImagenCreateDto imagenCreate)
        {
            var operationResult = OperationResult.CreateForValidation();
            // El SOPInstanceUID es crucial para la identificación única de la imagen dentro de la serie, y no debe estar vacío.
            if (string.IsNullOrWhiteSpace(imagenCreate.SOPInstanceUID))
                operationResult.AddError("Modality no puede ser vacío.", imagenCreate.SOPInstanceUID);
            // La validación del número de imagen asegura que se ha proporcionado un valor.
            if (imagenCreate.ImageNumber <= 0)
                operationResult.AddError("No contiene imagen.", null);
            // La interpretación fotométrica describe cómo se interpretan los datos de píxeles, no debe estar vacía si es crucial para tu procesamiento de imágenes.
            // Si es un campo opcional para tu lógica de negocio, esta validación puede ser omitida o ajustada.
            if (string.IsNullOrWhiteSpace(imagenCreate.PhotometricInterpretation))
                operationResult.AddError("No contiente información para interpretación fotometrica.", null);
            // Las dimensiones de la imagen (Rows y Columns) 
            if (imagenCreate.Rows <= 0 || imagenCreate.Columns <= 0)
                operationResult.AddError("Información de dimensiones no validas.", null);
            return operationResult;
        }

        internal OperationResult IsValidPacienteCreate(PacienteCreateDto pacienteCreate)
        // 15/03/24 Cambio retorno de bool a OperationResult
        {
            var operationResult = OperationResult.CreateForValidation();
            //Identificador de paciente
            if (string.IsNullOrWhiteSpace(pacienteCreate.GeneratedPatientID))
                operationResult.AddError("El ID del paciente no puede estar vacío.", null);
            //Nombre de paciente
            if (string.IsNullOrWhiteSpace(pacienteCreate.PatientName))
                operationResult.AddError("El nombre del paciente no puede estar vacío.", null);
            return operationResult;
        }

        internal OperationResult IsValidSerieCreate(SerieCreateDto serieCreate)
        {
            var operationResult = OperationResult.CreateForValidation();
            // El SeriesInstanceUID es crucial para la identificación única de la serie dentro del estudio, no debe estar vacío.
            if (string.IsNullOrWhiteSpace(serieCreate.SeriesInstanceUID))
                operationResult.AddError("No contiene imagen.", null);
            // La modalidad indica el tipo de equipo que generó la serie. Es un dato crucial y no debe estar vacío.
            if (string.IsNullOrWhiteSpace(serieCreate.Modality))
                operationResult.AddError("No contiene modalidad.", null);
            // La fecha y hora de la serie (SeriesDateTime) podría ser opcional dependiendo de la implementación, pero si se provee, debería ser una fecha válida y no futura.
            if (serieCreate.SeriesDateTime.HasValue && serieCreate.SeriesDateTime > DateTime.Now)
                operationResult.AddError("fecha invalida.", null);
            return operationResult;
        }

        public string Token
        {
            get
            {
                if (string.IsNullOrEmpty(_token))
                {
                    _token = _httpContextAccessor.HttpContext?.Session.GetString(DS.SessionToken);
                }
                return _token;
            }
        }

    }
}
