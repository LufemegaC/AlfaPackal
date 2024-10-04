using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;

namespace WorkStation.Models
{
    public class ModalitySCU 
    {
        private readonly IDicomClient _dicomClient;

        public string IPAddress { get; }
        public int Port { get; }
        public bool UseTls { get; }
        public string CallingAe { get; }
        public string CalledAe { get; }

        public ModalitySCU(string ipAddress, int port, bool useTls, string callingAe, string calledAe)
        {
            IPAddress = ipAddress;
            Port = port;
            UseTls = useTls;
            CallingAe = callingAe;
            CalledAe = calledAe;
            _dicomClient = DicomClientFactory.Create(ipAddress, port, useTls, callingAe, calledAe);
        }

        public async Task SendAsync(DicomRequest request)
        {
            _dicomClient.NegotiateAsyncOps();
            // Agrega 5 solicitudes C-ECHO al cliente para solicitar coneccion
            for (int i = 0; i < 5; i++)
            {
                await _dicomClient.AddRequestAsync(new DicomCEchoRequest());
            }
            await _dicomClient.AddRequestAsync(request);
            await _dicomClient.SendAsync();
        }

        public async Task SendEcho()
        {
            _dicomClient.NegotiateAsyncOps();
            // Agrega 5 solicitudes C-ECHO al cliente para solicitar coneccion
            for (int i = 0; i < 5; i++)
            {
                await _dicomClient.AddRequestAsync(new DicomCEchoRequest());
            }
            await _dicomClient.SendAsync();
        }
    }
}
