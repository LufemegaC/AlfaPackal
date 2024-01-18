using AutoMapper;
using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using InterfazBasica.Service.IService;
using InterfazBasica_DCStore.Models.ViewModels;
using InterfazBasica_DCStore.Service.DicomServices;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using static Utileria.GeneralFunctions;

namespace InterfazBasica_DCStore.Controllers
{
    public class GatewayController : Controller
    {
        //public DicomServer<CStoreSCP> dicomServer{ get; set; }
        private IDicomServer _dicomServer;


        //Constructor
        public GatewayController( IDicomServer dicomServer)
        {
            //Asignaacion de dependencias
            _dicomServer = dicomServer;

        }
        public IActionResult Index()
        {
            var viewModelServer = new DicomServerVM
            {
                Host = _dicomServer.IPAddress,
                Port = _dicomServer.Port,
                IsListening = _dicomServer.IsListening,
                Estatus = _dicomServer.IsListening ? "Escuchando" : "Desactivado"
        };

            return View(viewModelServer);
        }

        public IActionResult Stop()
        {
            _dicomServer.Stop();
            return View();
        }
        public async Task<IActionResult> Start(DicomServerVM viewModelServer)
        {
            try { 
                if(_dicomServer.IsListening)
                {
                    return View("Index");
                }
                
                await _dicomServer.StartAsync(
                   GetLocalIPAddress(),// Direccion IP Local
                   GetServerPort(0),  // Puerto a utilizar
                   null, // ITlsAcceptor para conexiones TLS (Pendiente)
                   Encoding.UTF8, //  Codificación de reserva
                   new DicomServiceOptions(), // Opciones de servicio
                   null // Estado de usuario adicional
                );
                viewModelServer.IsListening = _dicomServer.IsListening;
                return View(viewModelServer);
            }
            catch (DicomNetworkException ex) when (ex.Message.Contains("cannot be started again"))
            {
                // Manejar específicamente la excepción de servidor ya iniciado
                // Log y acciones correspondientes
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _dicomServer.Stop();
                return RedirectToAction("Index");
            }
        }
    }
}
