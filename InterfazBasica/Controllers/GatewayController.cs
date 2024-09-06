using FellowOakDicom.Network;
using InterfazBasica_DCStore.Models.DicomServers;
using InterfazBasica_DCStore.Models.Dtos.Server;
using InterfazBasica_DCStore.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InterfazBasica_DCStore.Controllers
{
    public class GatewayController : Controller
    {
        //public DicomServer<CStoreSCP> dicomServer{ get; set; }
        //19/01/2024 Luis Felipe MG Delego la responsabilidad del servidor a ServerCStore
        //private IDicomServer _dicomServer { get; set; }
        private static DicomServerDto _dicomServerDto { get; set; }

        public IActionResult ServerConfig()
        {
            if (TempData["DicomServer"] != null)
            {
                _dicomServerDto = JsonConvert.DeserializeObject<DicomServerDto>(TempData["DicomServer"].ToString());
                _dicomServerDto.IsRunning = ServerCStore.IsRunning;
            }
            return View(_dicomServerDto);
        }

        public IActionResult Stop()
        {
            ServerCStore.Stop();
            _dicomServerDto.IsRunning = false;
            return View("ServerConfig", _dicomServerDto);
        }

        public async Task<IActionResult> Start()
        {
            try 
            {
                // Inicializo el servidor
                DicomUtility.InitializeCustomDicomStatuses();
                ServerCStore.Start(_dicomServerDto.Port,
                                   _dicomServerDto.AETitle);
                _dicomServerDto.IsRunning = true;
                
                return View("ServerConfig", _dicomServerDto);
            }
            catch (DicomNetworkException ex) when (ex.Message.Contains("cannot be started again"))
            {
                // Manejar específicamente la excepción de servidor ya iniciado
                // Log y acciones correspondientes
                return RedirectToAction("Index", _dicomServerDto);
            }
            catch (Exception ex)
            {
                ServerCStore.Stop();
                return RedirectToAction("Index", _dicomServerDto);
            }
        }
    }
}
