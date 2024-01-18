using FellowOakDicom.Network;
using FellowOakDicom;
using FellowOakDicom.Network.Client;
using Microsoft.AspNetCore.Mvc;
using WorkStation.Models.ViewModels;
using static Utileria.GeneralFunctions;

namespace WorkStation.Controllers
{
    public class WorkstationController : Controller
    {
        [BindProperty]
        private IDicomClient _client { get; set; }
        public WorkstationController(IDicomClient client)
        {
            _client = client;
        }

        public IActionResult Index()
        {
            var viewModelClient = new DicomClienteVM
            {
                Host = _client.Host,
                Port = _client.Port, // Puerto
                CallingAe = _client.CallingAe, // AETitle Client
                CalledAe = _client.CalledAe
            };
            return View(viewModelClient);
        }

        [HttpPost]
        public async Task<IActionResult> SendDicomFile(IFormFile dicomFile)
        {
            if (dicomFile != null && dicomFile.Length > 0)
            {
                try
                {
                    using (var stream = dicomFile.OpenReadStream())
                    {
                        var dicomImage = DicomFile.Open(stream);
                        await _client.AddRequestAsync(new DicomCStoreRequest(dicomImage));
                        await _client.SendAsync();
                    }
                    ViewBag.Message = "Archivo DICOM enviado con éxito.";
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
            return View("Index");
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
            return View("Index");
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
