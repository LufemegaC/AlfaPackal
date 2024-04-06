using FellowOakDicom.Network;
using FellowOakDicom.Network.Tls;
using System.Text;

namespace InterfazBasica_DCStore.Models.Dicom
{
    public class DicomServerFactoryPruebas : IDicomServerFactory
    {
        //
        // Resumen:
        //     Creates a DICOM server object out of DI-container.
        //
        // Parámetros:
        //   port:
        //     Port to listen to.
        //
        //   tlsAcceptor:
        //     Handler to accept authenticated connections.
        //
        //   fallbackEncoding:
        //     Fallback encoding.
        //
        //   logger:
        //     Logger, if null default logger will be applied.
        //
        // Parámetros de tipo:
        //   T:
        //     DICOM service that the server should manage.
        //
        // Devuelve:
        //     An instance of FellowOakDicom.Network.DicomServer`1, that starts listening for
        //     connections in the background.
        public IDicomServer Create<T>(int port, ITlsAcceptor tlsAcceptor = null, Encoding fallbackEncoding = null, ILogger logger = null, object userState = null) where T : DicomService, IDicomServiceProvider
        {
            return ServiceProviderServiceExtensions.GetRequiredService<IDicomServerFactory>(SetupPruebas.ServiceProvider).Create<T>(port, tlsAcceptor, fallbackEncoding, logger, userState);
            //throw new NotImplementedException();
        }

        
       
        public IDicomServer Create<T>(string ipAddress, int port, ITlsAcceptor tlsAcceptor = null, Encoding fallbackEncoding = null, ILogger logger = null, object userState = null) where T : DicomService, IDicomServiceProvider
        {
            throw new NotImplementedException();
        }

        public IDicomServer Create<T, TServer>(string ipAddress, int port, object userState = null, ITlsAcceptor tlsAcceptor = null, Encoding fallbackEncoding = null, ILogger logger = null)
            where T : DicomService, IDicomServiceProvider
            where TServer : IDicomServer<T>
        {
            throw new NotImplementedException();
        }
    }
}
