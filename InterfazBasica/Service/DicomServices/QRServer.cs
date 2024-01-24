using FellowOakDicom.Network;
using Microsoft.AspNetCore.Components.Web;
using FellowOakDicom.Log;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public static class QRServer
    {
        private static IDicomServer _server;
        private static int _port; // Puerto (104)
        public static string AETitle;// AEt

        //public static IDicomImageFinderService CreateFinderService => new StupidSlowFinderService();
        public static IDicomImageFinderService CreateFinderService { get; private set; } = new StupidSlowFinderService();
        public static bool IsRunning => _server?.IsListening ?? false; //Bandera, Estatus del server
        public static string IPAddress => _server?.IPAddress ?? "Sin Asignar";
        public static void Start(int port, string aet)
        {
            //Logger.Info($"Servidor arriba AE: {aet}");
            Stop(); //Detengo servidor
            _port = port; //Puerto
            AETitle = aet; //AETitle
            _server = DicomServerFactory.Create<QRService>(port); //Levanta servidor
        }


        public static void Stop()
        {
            _server?.Dispose(); //valida si Esta arriba
            _server = null; // Stop server
        }

        public static void Restart()
        {
            Stop(); //Detener servidor
            Start(_port, AETitle); //Levantar servidor
        }

        public static void SetFinderService(IDicomImageFinderService finderService)
        {
            if (finderService == null)
            {
                throw new ArgumentNullException(nameof(finderService));
            }

            LogChange(finderService);
            CreateFinderService = finderService;
            NotifyFinderServiceChanged();
        }
        private static void LogChange(IDicomImageFinderService finderService)
        {
            // Aquí puedes agregar el código para registrar en un log el cambio de servicio.
            // Por ejemplo, podrías registrar el nombre del nuevo servicio y la hora del cambio.
            var message = $"Finder service changed to {finderService.GetType().Name} at {DateTime.UtcNow}";
            Console.WriteLine(message); // reemplaza esto con tu método de registro preferido
        }

        private static void NotifyFinderServiceChanged()
        {
            // Aquí puedes agregar el código para notificar a otras partes de tu aplicación que el servicio de búsqueda de imágenes ha cambiado.
            // Esto podría ser tan simple como llamar a un evento, o podría involucrar el envío de una notificación a través de un servicio de mensajería.
            // Por ahora, solo voy a imprimir un mensaje en la consola.
            Console.WriteLine("Finder service has been changed.");
        }

        public static void HandleException(Exception ex)
        {
            // Aquí puedes manejar la excepción de la manera que prefieras.
            // Por ejemplo, podrías registrarla en un archivo de log, mostrar un mensaje de error al usuario, etc.
        }
    }
}
