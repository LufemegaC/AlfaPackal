using FellowOakDicom.Network;

namespace InterfazBasica_DCStore.Service.DicomServers
{
    public class ServerCFind
    {
        private static IDicomServer _server;

        public static string AETitle { get; set; }


        //public static IDicomImageFinderService CreateFinderService => new StupidSlowFinderService();


        //public static void Start(int port, string aet)
        //{
        //    AETitle = aet;
        //    _server = DicomServerFactory.Create<QRService>(port);
        //}


        public static void Stop()
        {
            _server.Dispose();
        }


    }
}
