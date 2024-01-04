using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using InterfazBasica_DCStore.Models.ViewModels;
using InterfazBasica_DCStore.Service.DicomServices;
using Microsoft.AspNetCore.Mvc;
using static Utileria.GeneralFunctions;

namespace InterfazBasica_DCStore.Controllers
{
    public class GatewayController : Controller
    {
        //public DicomServer<CStoreSCP> dicomServer{ get; set; }
        [BindProperty]
        private IDicomServer _dicomServer { get; set; }

        public GatewayController(IDicomServer dicomServer)
        {
            _dicomServer = dicomServer;
        }
        public IActionResult Index()
        {
            var viewModelServer = new DicomServerVM
            {
                Host = _dicomServer.IPAddress,
                Port = _dicomServer.Port,
            };
            return View(viewModelServer);
        }
        //public IActionResult StopLocalServer()
        //{

        //}
    }
}
