using FellowOakDicom.Network;
using FellowOakDicom;
using FellowOakDicom.Network.Client;
using Microsoft.AspNetCore.Mvc;
using WorkStation.Models.ViewModels;
using static Utileria.GeneralFunctions;
using System.Security.Cryptography;

namespace WorkStation.Controllers
{
    public class WorkstationController : Controller
    {
        private readonly ILogger<WorkstationController> _logger;
        [BindProperty]
        private IDicomClient _client { get; set; }
        private DicomClienteVM _viewModelClient { get; set; }

        private const int BatchSize = 10; // Tamaño del lote
        public WorkstationController(IDicomClient client, ILogger<WorkstationController> logger)
        {
            _client = client;
            _viewModelClient = new DicomClienteVM
            {
                Host = _client.Host,
                Port = _client.Port, // Puerto
                Aet = _client.CallingAe, // AETitle Client
                CalledAe = _client.CalledAe
            };
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_viewModelClient);
        }

        [HttpPost] 
        public async Task<IActionResult> SendDicomFile(IFormFile dicomFile)
        {
            if (dicomFile != null && dicomFile.Length > 0)
            {
                try
                {
                    _client.NegotiateAsyncOps();
                    await _client.AddRequestAsync(new DicomCEchoRequest());
                    string errorMessage = "Proceso correcto";
                    // Event handler para manejar la respuesta de C-STORE
                    async Task OnCStoreResponseReceived(DicomCStoreRequest request, DicomCStoreResponse response)
                    {
                        // Aquí puedes verificar el estado de la respuesta
                        
                        if (response.Status == DicomStatus.DuplicateSOPInstance)
                        {
                            // Establece el mensaje correspondiente a duplicado
                            errorMessage = "La imagen DICOM ya está registrada.";
                             
                        }
                        else if (response.Status == DicomStatus.Success)
                        {
                            // Establece el mensaje de éxito
                            errorMessage = "Archivo DICOM enviado con éxito.";
                        }
                        else
                        {
                            // Otros casos, puedes usar response.Status.Description para obtener una descripción del estado
                            errorMessage = $"El servidor respondió con estado: {response.Status.Description}";
                        }

                        // Puedes usar este mensaje para actualizar el ViewBag o gestionar la respuesta
                        // Ten en cuenta que necesitas manejar el contexto de sincronización ya que estás dentro de un callback asincrónico
                    }

                    using (var stream = dicomFile.OpenReadStream())
                    {
                        var dicomImage = DicomFile.Open(stream);
                        //Codigo original
                        //await _client.AddRequestAsync(new DicomCStoreRequest(dicomImage));
                        //Codigo de pruebas
                        var dicomCSToreRequest = new DicomCStoreRequest(dicomImage);
                        await _client.AddRequestAsync(dicomCSToreRequest);
                        //
                        await _client.SendAsync();
                    }

                    ViewBag.Message = errorMessage;
                }
                catch (Exception ex)
                {
                    // Manejar excepción
                    ViewBag.Message = $"Error: {ex.Message}";
                }
            }
            else
            {
                ViewBag.Message = "Debe seleccionar un archivo DICOM";
            }
            return View("Index", _viewModelClient);
        }


        [HttpPost]
        public async Task<IActionResult> SendCdmFiles(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            {
                ViewBag.Message = "Debe seleccionar una carpeta válida con archivos DICOM";
                return View("Index", _viewModelClient);
            }

            try
            {
                _client.NegotiateAsyncOps();
                await _client.AddRequestAsync(new DicomCEchoRequest());
                string errorMessage = "Proceso correcto";

                // Obtener todos los archivos .dcm en la carpeta y sus subcarpetas
                var dicomFiles = Directory.GetFiles(folderPath, "*.dcm", SearchOption.AllDirectories);
                var fileCount = 0;

                foreach (var filePath in dicomFiles)
                {
                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            var dicomImage = DicomFile.Open(stream);
                            var request = new DicomCStoreRequest(dicomImage);
                            request.OnResponseReceived += (DicomCStoreRequest req, DicomCStoreResponse res) =>
                            {
                                if (res.Status == DicomStatus.DuplicateSOPInstance)
                                {
                                    errorMessage = "La imagen DICOM ya está registrada.";
                                }
                                else if (res.Status == DicomStatus.Success)
                                {
                                    errorMessage = "Archivo DICOM enviado con éxito.";
                                }
                                else
                                {
                                    errorMessage = $"El servidor respondió con estado: {res.Status.Description}";
                                }
                            };
                            await _client.AddRequestAsync(request);
                        }

                        fileCount++;

                        // Enviar el lote cuando se alcanza el tamaño de lote
                        if (fileCount % BatchSize == 0)
                        {
                            await _client.SendAsync();
                            //_client.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Registrar el error y continuar con el siguiente archivo
                        _logger.LogError(ex, $"Error procesando el archivo {filePath}");
                    }
                }

                // Enviar cualquier archivo restante en el último lote
                if (fileCount % BatchSize != 0)
                {
                    await _client.SendAsync();
                }

                ViewBag.Message = errorMessage;
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
                _logger.LogError(ex, "Error en el procesamiento de archivos DICOM.");
            }

            return View("Index", _viewModelClient);
        }


        public async Task<IActionResult> Connect()
        {
            try
            {
                _client.NegotiateAsyncOps();
                // Agrega diez solicitudes C-ECHO al cliente para solicitar coneccion
                for (int i = 0; i < 5; i++)
                {
                    await _client.AddRequestAsync(new DicomCEchoRequest());
                }
                await _client.SendAsync();
                foreach (DicomPresentationContext ctr in _client.AdditionalPresentationContexts)
                {
                    Console.WriteLine("PresentationContext: " + ctr.AbstractSyntax + " Result: " + ctr.Result);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error al conectar: {ex.Message}";
            }
            return View("Index", _viewModelClient);
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
    }
}
