using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Network;
using InterfazBasica.Models;
using InterfazBasica_DCStore.Models;
using InterfazBasica_DCStore.Service.IDicomService;
using InterfazBasica_DCStore.Service.IService;
using System.Net;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class DicomOrchestrator : IDicomOrchestrator
    {
        private readonly IDicomValidationService _validationService;
        private readonly IDicomDecompositionService _decompositionService;
        private readonly IServiceAPI _serviceAPI;
        
        //private readonly IDicomImageFinderService _dicomImageFinderService;

        private string _rootPath;
        public DicomOrchestrator(
            IDicomValidationService validationService,
            IDicomDecompositionService decompositionService,
            IServiceAPI serviceAPI,
            IConfiguration configuration)
        {
            _validationService = validationService;
            _decompositionService = decompositionService;
            _serviceAPI = serviceAPI;
            _rootPath = configuration.GetValue<string>("DicomSettings:StoragePath");
            //services.Configure<DicomSettings>(Configuration.GetSection("DicomSettings"));

        }

        public async Task<DicomStatus> StoreDicomData(DicomFile dicomFile)
        {
            // Validación inicial del archivo DICOM.
            var dicomStatus = _validationService.IsValidDicomFile(dicomFile);
            if (dicomStatus != DicomStatus.Success)
                return dicomStatus;
            var mainDicomValues = ExtractMainValues(dicomFile.Dataset);
            ////    ***************************
            mainDicomValues = await _validationService.ValidateEntities(mainDicomValues);
            //Validate duplicate dicom
            if (mainDicomValues.ExistImage)
                return DicomStatus.DuplicateSOPInstance;
            var metadata = _decompositionService.ExtractMetadata(dicomFile.Dataset);
            if (mainDicomValues.ExistStudy)// Study already exist
            {
                if (mainDicomValues.ExistSerie)
                {
                    //Register image
                    var imageCreateDto = await _decompositionService.DecomposeDicomToImagen(metadata);
                    imageCreateDto.PACS_SerieID = mainDicomValues.PACS_SerieID;
                    imageCreateDto.ImageLocation = RootFileConstructor(mainDicomValues);
                    var apiRespRegImg = await _serviceAPI.RegistrarImagen(imageCreateDto);
                    if (apiRespRegImg != null && apiRespRegImg.PacsResourceId == 0)
                        return TranslateApiResponseToDicomStatus(apiRespRegImg);
                }
                else
                {
                    //Register Serie
                    var serieCreateDto = await _decompositionService.DecomposeDicomToSerie(metadata);
                    serieCreateDto.PACS_EstudioID = mainDicomValues.PACS_EstudioID;
                    var apiRespRegSer = await _serviceAPI.RegistrarSerie(serieCreateDto);
                    if (apiRespRegSer != null && apiRespRegSer.PacsResourceId == 0)
                        return TranslateApiResponseToDicomStatus(apiRespRegSer);
                    //Register image
                    var imageCreateDto = await _decompositionService.DecomposeDicomToImagen(metadata);
                    imageCreateDto.PACS_SerieID = apiRespRegSer.PacsResourceId;
                    imageCreateDto.ImageLocation = RootFileConstructor(mainDicomValues);
                    var apiRespRegImg = await _serviceAPI.RegistrarImagen(imageCreateDto);
                    if (apiRespRegImg != null && apiRespRegImg.PacsResourceId == 0)
                        return TranslateApiResponseToDicomStatus(apiRespRegImg);
                }
            }
            else
            {
                //Mapeo de entidades
                var pacienteCreateDto = await _decompositionService.DecomposeDicomToPaciente(metadata);
                var estudioCreateDto = await _decompositionService.DecomposeDicomToEstudio(metadata);
                var serieCreateDto = await _decompositionService.DecomposeDicomToSerie(metadata);
                var imageCreateDto = await _decompositionService.DecomposeDicomToImagen(metadata);
                //-- PACIENTE
                var apiRespRegPac = await _serviceAPI.RegistrarPaciente(pacienteCreateDto);
                if (apiRespRegPac != null && apiRespRegPac.PacsResourceId == 0)
                    return TranslateApiResponseToDicomStatus(apiRespRegPac);
                //return DicomStatus.Cancel;
                //-- ESTUDIO
                //recupero IDs del registro del paciente
                estudioCreateDto.PACS_PatientID = apiRespRegPac.PacsResourceId; // ID Generado por la BDs
                estudioCreateDto.GeneratedPatientID = apiRespRegPac.GeneratedServId;
                //Asignacion de ruta:
                //estudioCreateDto.DicomFileLocation 
                var apiRespRegEst = await _serviceAPI.RegistrarEstudio(estudioCreateDto);
                if (apiRespRegEst != null && apiRespRegEst.PacsResourceId == 0)
                    return DicomStatus.Cancel;
                //--SERIE
                serieCreateDto.PACS_EstudioID = apiRespRegEst.PacsResourceId;
                var apiRespRegSer = await _serviceAPI.RegistrarSerie(serieCreateDto);
                if (apiRespRegSer != null && apiRespRegSer.PacsResourceId == 0)
                    return DicomStatus.Cancel;
                //--IMAGEN
                imageCreateDto.PACS_SerieID = apiRespRegSer.PacsResourceId;
                imageCreateDto.ImageLocation = RootFileConstructor(mainDicomValues);
                var apiRespRegImg = await _serviceAPI.RegistrarImagen(imageCreateDto);
                if (apiRespRegImg != null && apiRespRegImg.PacsResourceId == 0)
                    return DicomStatus.Cancel;
            }
            //Proceso correcto
            return DicomStatus.Success;
        }


        public async Task<DicomStatus> StoreDicomFile(DicomFile dicomFile)
        {

            var mainDicomValues = ExtractMainValues(dicomFile.Dataset);
            var filePath = RootFileConstructor(mainDicomValues);
            // Guarda el archivo DICOM en la ruta especificada.
            await dicomFile.SaveAsync(filePath);
            // Retorna una respuesta de éxito.
            return DicomStatus.Success;

        }

        internal MainEntitiesValues ExtractMainValues(DicomDataset dicomDataset)
        {
            var mainEntitiesValues = new MainEntitiesValues
            {
                PatientID = dicomDataset.GetSingleValueOrDefault<string>(DicomTag.PatientID, ""),
                IssuerOfPatientID = dicomDataset.GetSingleValueOrDefault<string>(DicomTag.IssuerOfPatientID, ""),
                StudyInstanceUID = dicomDataset.GetSingleValueOrDefault<string>(DicomTag.StudyInstanceUID, ""),
                SeriesInstanceUID = dicomDataset.GetSingleValueOrDefault<string>(DicomTag.SeriesInstanceUID, ""),
                SOPInstanceUID = dicomDataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, ""),   
            };
            //if (dicomDataset.Contains(DicomTag.NumberOfFrames))
            //{
            //    mainEntitiesValues.NumberOfFrames = dicomDataset.GetValue<int>(DicomTag.NumberOfFrames, 0);
            //}
            return mainEntitiesValues;
        }

        internal DicomStatus TranslateApiResponseToDicomStatus(APIResponse apiResponse)
        {
            // Caso de éxito
            if (apiResponse.StatusCode == HttpStatusCode.OK && apiResponse.IsExitoso)
            {
                return DicomStatus.Success;
            }

            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return DicomStatus.InvalidArgumentValue;
                case HttpStatusCode.NotFound:
                    return DicomStatus.NoSuchObjectInstance;
                case HttpStatusCode.InternalServerError:
                    return DicomStatus.ProcessingFailure;
                case HttpStatusCode.ServiceUnavailable:
                    return DicomStatus.ResourceLimitation;
                default:
                    // Un mapeo genérico para otros casos
                    return DicomStatus.ProcessingFailure;
            }
        }


        internal string RootFileConstructor(MainEntitiesValues mainValues)
        {
            // Construye la ruta completa usando StudyInstanceUID y SeriesInstanceUID.
            var fullPath = Path.Combine(_rootPath, mainValues.StudyInstanceUID, mainValues.SeriesInstanceUID);
            // Verifica si la ruta del directorio existe, si no, la crea.
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            // Completa la ruta del archivo añadiendo el SOPInstanceUID y la extensión .dcm.
            var filePath = Path.Combine(fullPath, mainValues.SOPInstanceUID + ".dcm");
            return filePath;
        }
        ///

        public void ProcesarFrameEspecifico(DicomDataset dicomDataset, int indiceFrame)
        {
            // Cargar el archivo DICOM
            //var dicomFile = DicomFile.Open(rutaArchivoDicom);

            // Asegurarte de que el archivo es multiframe comprobando el número de frames
            var numeroDeFrames = dicomDataset.GetValueOrDefault<int>(DicomTag.NumberOfFrames, 0, 1);
            if (numeroDeFrames > 1)
            {
                // Acceder al frame específico
                // Nota: Los índices en fo-dicom comienzan en 0, asegúrate de ajustar el índice si es necesario
                var frame = DicomPixelData.Create(dicomDataset).GetFrame(indiceFrame - 1);
                // Crear un objeto DicomImage a partir del frame
                var imagen = new DicomImage(dicomDataset, 1);
                //// Renderizar el frame a un objeto de imagen (por ejemplo, para visualización o para guardar en otro formato)
                //// Aquí se guarda el frame como una imagen JPEG
                //var rutaImagenSalida = Path.ChangeExtension(rutaArchivoDicom, $"_frame{indiceFrame}.jpeg");
                var frameimage = imagen.RenderImage(1);
                //imagen.RenderImage().AsClonedBitmap().Save(rutaImagenSalida, System.Drawing.Imaging.ImageFormat.Jpeg);
                //Console.WriteLine($"Frame {indiceFrame} guardado como imagen JPEG en: {rutaImagenSalida}");
            }
            else
            {
                Console.WriteLine("El archivo DICOM no es multiframe o el número de frames no está especificado.");
            }
        }

    }
}
