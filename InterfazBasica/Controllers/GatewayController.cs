using AutoMapper;
using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using InterfazBasica.Service.IService;
using InterfazBasica_DCStore.Models.Pacs;
using InterfazBasica_DCStore.Models.ViewModels;
using InterfazBasica_DCStore.Service.DicomServers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Text;
using static Utileria.GeneralFunctions;

namespace InterfazBasica_DCStore.Controllers
{
    public class GatewayController : Controller
    {
        //public DicomServer<CStoreSCP> dicomServer{ get; set; }
        //19/01/2024 Luis Felipe MG Delego la responsabilidad del servidor a ServerCStore
        //private IDicomServer _dicomServer { get; set; }
        private static DicomServerDto _dicomServerDto { get; set; }
        private static  DicomServerVM _dicomServerVM { get; set; }

        static GatewayController()
        {
            
        }

        public IActionResult MenuPrincipal()
        {
            if (TempData["DicomServer"] != null)
            { 
                var dicomServer = JsonConvert.DeserializeObject<DicomServerDto>(TempData["DicomServer"].ToString());
                _dicomServerVM = new DicomServerVM
                {
                    DicomServerDto = dicomServer,
                    IsListening = ServerCStore.IsRunning,
                    Estatus = ServerCStore.IsRunning ? "Escuchando" : "Desactivo",
                };
            }
            return View(_dicomServerVM);
        }

        public IActionResult Stop()
        {
            ServerCStore.Stop();
            return View(_dicomServerDto);
        }

        public async Task<IActionResult> Start(DicomServerVM dicomServerVM)
        {
            try 
            {
                // Inicializo el servidor
                ServerCStore.Start(dicomServerVM.DicomServerDto.PuertoRedLocal, 
                                   dicomServerVM.DicomServerDto.AETitle);
                dicomServerVM.IsListening = ServerCStore.IsRunning;
                dicomServerVM.Estatus = ServerCStore.IsRunning ? "Escuchando" : "Desactivado";
                return View("MenuPrincipal", dicomServerVM);
            }
            catch (DicomNetworkException ex) when (ex.Message.Contains("cannot be started again"))
            {
                // Manejar específicamente la excepción de servidor ya iniciado
                // Log y acciones correspondientes
                return RedirectToAction("Index", _dicomServerVM);
            }
            catch (Exception ex)
            {
                ServerCStore.Stop();
                return RedirectToAction("Index", _dicomServerVM);
            }
        }
    }
}
