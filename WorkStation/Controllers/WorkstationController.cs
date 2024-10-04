using FellowOakDicom.Network;
using FellowOakDicom;
using FellowOakDicom.Network.Client;
using Microsoft.AspNetCore.Mvc;
using WorkStation.Models;
using WorkStation.Models.ViewModels;
using Newtonsoft.Json;

namespace WorkStation.Controllers
{
    public class WorkstationController : Controller
    {
        private readonly ILogger<WorkstationController> _logger;
        private readonly ModalitySCU _modalityScu;

        private const int BatchSize = 1; // Tamaño del lote
        public WorkstationController(ILogger<WorkstationController> logger, ModalitySCU modalityScu)
        {
            _modalityScu = modalityScu;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Construir el ViewModel usando la información de ModalitySCU
            var modalityVm = new ModalityVM
            {
                DicomClient = new DicomClientDto
                {
                    IP = _modalityScu.IPAddress,
                    Port = _modalityScu.Port,
                    AETitle = _modalityScu.CallingAe,
                    CalledAE = _modalityScu.CalledAe
                }
            };
            // Almacenar un objeto en la sesión (serializado a JSON)
            HttpContext.Session.SetString("ModalityVM", JsonConvert.SerializeObject(modalityVm));
            return View(modalityVm);
        }

        [HttpPost]
        public async Task<IActionResult> SendDicomFile(IFormFile dicomFile)
        {
            // Recuperar el ModalityVM desde la sesión
            var modalityVm = JsonConvert.DeserializeObject<ModalityVM>(HttpContext.Session.GetString("ModalityVM"));
            if (dicomFile != null && dicomFile.Length > 0)
            {
                using (var stream = dicomFile.OpenReadStream())
                {
                    var resultMessage = await ProcessDicomFile(stream);
                    ViewBag.Message = resultMessage;
                }
            }
            else
            {
                ViewBag.Message = "Debe seleccionar un archivo DICOM";
            }
            return View("Index", modalityVm);
        }

        [HttpPost]
        public async Task<IActionResult> SendCdmFiles(string folderPath)
        {
            // Recuperar el ModalityVM desde la sesión
            var modalityVm = JsonConvert.DeserializeObject<ModalityVM>(HttpContext.Session.GetString("ModalityVM"));

            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            {
                ViewBag.Message = "Debe seleccionar una carpeta válida con archivos DICOM";
                return View("Index", modalityVm);
            }
            try
            {
                var dicomFiles = GetDicomFromDirectory(folderPath);
                foreach (var filePath in dicomFiles)
                {
                    try
                    {
                        using (var stream = System.IO.File.OpenRead(filePath))
                        {
                            var resultMessage = await ProcessDicomFile(stream);
                            _logger.LogInformation(resultMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error procesando el archivo {filePath}");
                        //dicomStatusList.Add(new DicomStatus { Description = $"Error en archivo {filePath}: {ex.Message}" });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
                _logger.LogError(ex, "Error en el procesamiento de archivos DICOM.");
            }

            return View("Index", modalityVm);
        }


        public async Task<IActionResult> Connect()
        {
            try
            {
                // Recuperar el ModalityVM desde la sesión
                var modalityVm = JsonConvert.DeserializeObject<ModalityVM>(HttpContext.Session.GetString("ModalityVM"));
                await _modalityScu.SendEcho();
                return View("Index", modalityVm);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error al conectar: {ex.Message}";
                return View("Index", null);
            }
            
        }

        public async Task<IActionResult> Disconnect()
        {
            try
            {
               // await _client.ReleaseAsync();
                ViewBag.Message = "Desconexión realizada con éxito.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error al desconectar: {ex.Message}";
            }
            return View("Index");
        }

        public IActionResult Exit()
        {
            return View("Index");
        }


        // Funcion de almacenamiento 
        private async Task<string> ProcessDicomFile(Stream dicomFileStream)
        {
            try
            {
                string errorMessage = "Proceso correcto";
                async Task OnCStoreResponseReceived(DicomCStoreRequest request, DicomCStoreResponse response)
                {
                    if (response.Status == DicomStatus.DuplicateSOPInstance)
                    {
                        errorMessage = "La imagen DICOM ya está registrada.";
                    }
                    else if (response.Status == DicomStatus.Success)
                    {
                        errorMessage = "Archivo DICOM enviado con éxito.";
                    }
                    else
                    {
                        errorMessage = $"El servidor respondió con estado: {response.Status.Description}";
                    }
                }

                var dicomImage = DicomFile.Open(dicomFileStream);
                var dicomCStoreRequest = new DicomCStoreRequest(dicomImage);
                await _modalityScu.SendAsync(dicomCStoreRequest);

                return errorMessage;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }


        static List<string> GetDicomFromDirectory(string path)
        {
            List<string> dicomFiles = new List<string>();

            GetDicomFilesRecursive(path, dicomFiles);

            return dicomFiles;
        }
        // Funcion recursiva para recorrer directorio
        static void GetDicomFilesRecursive(string path, List<string> dicomFiles)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (Path.GetExtension(file).Equals(".dcm", StringComparison.OrdinalIgnoreCase))
                {
                    dicomFiles.Add(file);
                }
            }

            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                GetDicomFilesRecursive(directory, dicomFiles);
            }
        }
    }
}
