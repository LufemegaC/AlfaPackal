using FellowOakDicom.Network;
using InterfazBasica_DCStore.Service.DicomServices;

namespace InterfazBasica_DCStore.Models.DicomServers
{
    public static class ServerCStore
    {
        private static IDicomServer _dicomServer;
        private static int _port; // Puerto, por ejemplo 104
        public static string Aetitle; // AETitle
        public static bool IsRunning => _dicomServer?.IsListening ?? false;

        public static void Start(int port, string AETitle)
        {
            Stop(); // Detiene el servidor si ya está en ejecución
            _port = port;
            Aetitle = AETitle;
            _dicomServer = DicomServerFactory.Create<CStoreSCP>(port);
        }


        public static void Stop()
        {
            _dicomServer?.Dispose();
            _dicomServer = null;
        }

        public static void Restart()
        {
            Stop();
            Start(_port, Aetitle);
        }



    }
}
