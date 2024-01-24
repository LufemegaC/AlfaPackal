using AutoMapper;
using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using InterfazBasica.Service.IService;
using InterfazBasica_DCStore.Models.ViewModels;
using InterfazBasica_DCStore.Service.DicomServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
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
        private static DicomServerVM _dicomServerVM { get; set; }


        static GatewayController()
        {
            _dicomServerVM = new DicomServerVM
            {
                Host = "Sin Asignar",
                Port = 0,
                IsListening = ServerCStore.IsRunning,
                Estatus = ServerCStore.IsRunning ? "Escuchando" : "Desactivo",
                Aet = "Sin Asignar"
            };
        }

        public IActionResult Index()
        {
            return View(_dicomServerVM);
        }

        public IActionResult Stop()
        {
            //_dicomServer.Stop();
            ServerCStore.Stop();
            return View(_dicomServerVM);
        }

        public async Task<IActionResult> ClienteLocal()
        {
            //_dicomServer.Stop();
            var client = DicomClientFactory.Create(GetLocalIPAddress(), 11112, false, "SCU_2", "STORESCP");
            client.NegotiateAsyncOps();
            for (int i = 0; i < 3; i++)
            {
                await client.AddRequestAsync(new DicomCEchoRequest());
            }
            await client.SendAsync();
            /*
            client.NegotiateAsyncOps();
            // Agrega diez solicitudes C-ECHO al cliente para solicitar coneccion
            for (int i = 0; i < 10; i++)
            {
                await client.AddRequestAsync(new DicomCEchoRequest());
            }
            await client.SendAsync(); // Envía todas las solicitudes al servidor.
            */
            foreach (DicomPresentationContext ctr in client.AdditionalPresentationContexts)
            {
                Console.WriteLine("PresentationContext: " + ctr.AbstractSyntax + " Result: " + ctr.Result);
            }
            return View("Index",_dicomServerVM);
        }

        public async Task<IActionResult> PruebaQRService()
        {
            //QRServer.Start(11112, "STORESCP");
            ServerCStore.Start(11112, "STORESCP");
            // Habilita la capacidad de operaciones asíncronas para el cliente Dicom.
            //_client.NegotiateAsyncOps();
            /*---------------------------------------------------------------*/

            var client = DicomClientFactory.Create(GetLocalIPAddress(), 11112, false, "SCU", "STORESCP");
            client.NegotiateAsyncOps();
            for (int i = 0; i < 3; i++)
            {
                await client.AddRequestAsync(new DicomCEchoRequest());
            }
            await client.SendAsync(); // Envía todas las solicitudes al servidor.
            //QRServer.Stop();
            return View("Index", _dicomServerVM);
        }

            public async Task<IActionResult> Start()
        {
            try {
                // 19/01/2024 Luis Felipe MG - Delegacion de puesto
                //if(ServerCStore.IsRunning)
                //{
                //    return View("Index", _dicomServerVM);
                //}
                //await _dicomServer.StartAsync(
                //   GetLocalIPAddress(),// Direccion IP Local
                //   GetServerPort(0),  // Puerto a utilizar
                //   null, // ITlsAcceptor para conexiones TLS (Pendiente)
                //   Encoding.UTF8, //  Codificación de reserva
                //   new DicomServiceOptions(), // Opciones de servicio
                //   null // Estado de usuario adicional
                //);
                ServerCStore.Start(1112,"STORESCP");
                _dicomServerVM.IsListening = ServerCStore.IsRunning;
                _dicomServerVM.Estatus = ServerCStore.IsRunning ? "Escuchando" : "Desactivado";
                _dicomServerVM.Port = 1112;
                _dicomServerVM.Host = "0.0.0.0";
                return View("Index", _dicomServerVM);
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
