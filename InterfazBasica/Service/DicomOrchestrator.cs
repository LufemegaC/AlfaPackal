using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Service.IService;
using InterfazBasica_DCStore.Service.IService.Dicom;
using InterfazBasica_DCStore.Utilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace InterfazBasica_DCStore.Service
{
    public class DicomOrchestrator : IDicomOrchestrator
    {
        private readonly IDicomValidationService _validationService;
        private readonly IDicomDecompositionService _decompositionService;
        private readonly IDicomWebService _dicomWebService;
        private string _rootPath;
        private int _institutionID; //Token de autorizacion
                               //private readonly IDicomImageFinderService _dicomImageFinderService;


        public DicomOrchestrator(
            IDicomValidationService validationService,
            IDicomDecompositionService decompositionService,
            IDicomWebService dicomWebService,
            IConfiguration configuration)
        {
            _validationService = validationService;
            _decompositionService = decompositionService;
            _dicomWebService = dicomWebService;
            _rootPath = configuration.GetValue<string>("DicomSettings:StoragePath");
            //services.Configure<DicomSettings>(Configuration.GetSection("DicomSettings"));

        }

        public async Task<DicomStatus> StoreDicomData(DicomFile dicomFile)
        {
            try
            {
                // Validación inicial del archivo DICOM.
                var dicomStatus = _validationService.IsValidDicomFile(dicomFile);
                if (dicomStatus != DicomStatus.Success)
                    return dicomStatus;
                var fileSizeMb = _decompositionService.GetFileSizeInMB(dicomFile);
                // Preparar el contenido multipart/form-data
                var content = new MultipartFormDataContent();

                // Convertir el DicomFile a un arreglo de bytes
                byte[] dicomFileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    dicomFile.Save(memoryStream);
                    dicomFileBytes = memoryStream.ToArray();
                }

                // Crear el contenido del archivo DICOM
                var dicomFileContent = new ByteArrayContent(dicomFileBytes);
                dicomFileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/dicom");
                content.Add(dicomFileContent, "dicomFile", "instance.dcm");

                var metadataDictionary = _decompositionService.ExtractMetadata(dicomFile.Dataset);
                var metadataDto = _decompositionService.DicomDictionaryToCreateEntities(metadataDictionary);
                metadataDto.TotalFileSizeMB = fileSizeMb;
                metadataDto.InstitutionID = LocalUtility.InstitutionId;
                var validationResult = _validationService.ValidateCreateDtos(metadataDto);
                if (validationResult != DicomStatus.Success)
                    return dicomStatus;

                var metadataJson = JsonConvert.SerializeObject(metadataDto);

                var metadataContent = new StringContent(metadataJson, Encoding.UTF8, "application/dicom+json");
                content.Add(metadataContent, "metadata");

                var apiResult = await _dicomWebService.RegisterInstances(content);
                
                // ** Pendiente manejo de error.
                //if (apiResult != null && apiResult.IsSuccessful == false)
                //{
                //    return LocalUtility.TranslateApiResponseToDicomStatus(apiResult);
                //}


                return DicomStatus.Success;
            }
            catch (Exception ex)
            {
                return new DicomStatus("A90E", DicomState.Failure, "Local server error:"+ ex.Message);
            }
           
        }

        public async Task<DicomStatus> StoreDicomFile(DicomFile dicomFile)
        {

            string instanceUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SOPInstanceUID, null);
            string studyUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.StudyInstanceUID, null);
            string seriesUID = dicomFile.Dataset.GetSingleValueOrDefault<string>(DicomTag.SeriesInstanceUID, null);
            var filePath = RootFileDicomConstructor(studyUID, seriesUID, instanceUID);
            // Guarda el archivo DICOM en la ruta especificada.
            await dicomFile.SaveAsync(filePath);
            // Retorna una respuesta de éxito.
            return DicomStatus.Success;

        }

        public string GetServerAEtitle()
        {
            return LocalUtility.Aetitle;
        }


        internal string RootFileDicomConstructor(string StudyUID, string SerieUID, string InstanceUID)
        {
            // Construye la ruta completa usando StudyInstanceUID y SeriesInstanceUID.
            var fullPath = Path.Combine(_rootPath, StudyUID, SerieUID);
            // Verifica si la ruta del directorio existe, si no, la crea.
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            // Completa la ruta del archivo añadiendo el SOPInstanceUID y la extensión .dcm.
            var filePath = Path.Combine(fullPath, InstanceUID + ".dcm");
            return filePath;
        }


        public void ProcesarFrameEspecifico(DicomDataset dicomDataset, int indiceFrame)
        {
            // Cargar el archivo DICOM
            //var dicomFile = DicomFile.Open(rutaArchivoDicom);

            // Asegurarte de que el archivo es multiframe comprobando el número de frames
            var numeroDeFrames = dicomDataset.GetValueOrDefault(DicomTag.NumberOfFrames, 0, 1);
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
