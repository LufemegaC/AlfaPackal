//Codigo para conocimiento de Ixchel DICOM, este codigo pertenece a la ultima version de fo-dicom 5.1.1
// o del proyecto de ejemplos 'fo-dicom-samples-master' 
//y su fin es ser complemento en el apoyo al usuario.


// clase CStoreSCP hereda de DicomService e implementa interfaces. Esta tomada del ejemplo C-Store SCP de 'fo-dicom-samples-master' 

public class CStoreSCP : DicomService, IDicomServiceProvider, IDicomCStoreProvider, IDicomCEchoProvider
    {
        private static readonly DicomTransferSyntax[] _acceptedTransferSyntaxes = new DicomTransferSyntax[]
            {
               DicomTransferSyntax.ExplicitVRLittleEndian,
               DicomTransferSyntax.ExplicitVRBigEndian,
               DicomTransferSyntax.ImplicitVRLittleEndian
            };

        private static readonly DicomTransferSyntax[] _acceptedImageTransferSyntaxes = new DicomTransferSyntax[]
        {
               // Lossless
               DicomTransferSyntax.JPEGLSLossless,
               DicomTransferSyntax.JPEG2000Lossless,
               DicomTransferSyntax.JPEGProcess14SV1,
               DicomTransferSyntax.JPEGProcess14,
               DicomTransferSyntax.RLELossless,
               // Lossy
               DicomTransferSyntax.JPEGLSNearLossless,
               DicomTransferSyntax.JPEG2000Lossy,
               DicomTransferSyntax.JPEGProcess1,
               DicomTransferSyntax.JPEGProcess2_4,
               // Uncompressed
               DicomTransferSyntax.ExplicitVRLittleEndian,
               DicomTransferSyntax.ExplicitVRBigEndian,
               DicomTransferSyntax.ImplicitVRLittleEndian
        };


        public CStoreSCP(INetworkStream stream, Encoding fallbackEncoding, ILogger log, DicomServiceDependencies dependencies)
            : base(stream, fallbackEncoding, log, dependencies)
        {
        }


        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            if (association.CalledAE != "STORESCP")
            {
                return SendAssociationRejectAsync(
                    DicomRejectResult.Permanent,
                    DicomRejectSource.ServiceUser,
                    DicomRejectReason.CalledAENotRecognized);
            }

            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification)
                {
                    pc.AcceptTransferSyntaxes(_acceptedTransferSyntaxes);
                }
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                {
                    pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
                }
            }

            return SendAssociationAcceptAsync(association);
        }


        public Task OnReceiveAssociationReleaseRequestAsync()
        {
            return SendAssociationReleaseResponseAsync();
        }


        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
            /* nothing to do here */
        }


        public void OnConnectionClosed(Exception exception)
        {
            /* nothing to do here */
        }


        public async Task<DicomCStoreResponse> OnCStoreRequestAsync(DicomCStoreRequest request)
        {
            var studyUid = request.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID).Trim();
            var instUid = request.SOPInstanceUID.UID;

            var path = Path.GetFullPath(DS.RutaAlmacen);
            path = Path.Combine(path, studyUid);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, instUid) + ".dcm";

            await request.File.SaveAsync(path);

            return new DicomCStoreResponse(request, DicomStatus.Success);
        }


        public Task OnCStoreRequestExceptionAsync(string tempFileName, Exception e)
        {
            // let library handle logging and error response
            return Task.CompletedTask;
        }


        public Task<DicomCEchoResponse> OnCEchoRequestAsync(DicomCEchoRequest request)
        {
            return Task.FromResult(new DicomCEchoResponse(request, DicomStatus.Success));
        }


// Interfaz IDicomServer 

namespace FellowOakDicom.Network
{
    //
    // Resumen:
    //     Interface representing a DICOM server instance.
    public interface IDicomServer : IDisposable
    {
        //
        // Resumen:
        //     Gets the IP address(es) the server listens to.
        string IPAddress { get; }

        //
        // Resumen:
        //     Gets the port to which the server is listening.
        int Port { get; }

        //
        // Resumen:
        //     Gets a value indicating whether the server is actively listening for client connections.
        bool IsListening { get; }

        //
        // Resumen:
        //     Gets the exception that was thrown if the server failed to listen.
        Exception Exception { get; }

        //
        // Resumen:
        //     Gets the options to control behavior of FellowOakDicom.Network.DicomService base
        //     class. Gets the port to which the server is listening.
        DicomServiceOptions Options { get; }

        //
        // Resumen:
        //     Gets the logger used by FellowOakDicom.Network.DicomServer`1
        ILogger Logger { get; set; }

        //
        // Resumen:
        //     Gets the service scope that will live as long as the DICOM server lives. Must
        //     be disposed alongside the DicomServer instance.
        IServiceScope ServiceScope { get; set; }

        //
        // Resumen:
        //     Gets the DICOM server registration ticket with the central registry. The registry
        //     prevents multiple DICOM servers from being created for the same IP address and
        //     port. This registration must be disposed alongside the DICOM server itself.
        DicomServerRegistration Registration { get; set; }

        //
        // Resumen:
        //     Starts the DICOM server listening for connections on the specified IP address(es)
        //     and port.
        //
        // Parámetros:
        //   ipAddress:
        //     IP address(es) for the server to listen to.
        //
        //   port:
        //     Port to which the server should be listening.
        //
        //   tlsAcceptor:
        //     Handler to accept secure connections.
        //
        //   fallbackEncoding:
        //     Encoding to apply if no encoding is identified.
        //
        //   options:
        //     Service options.
        //
        //   userState:
        //     User state to be shared with the connected services.
        //
        // Devuelve:
        //     Awaitable System.Threading.Tasks.Task.
        Task StartAsync(string ipAddress, int port, ITlsAcceptor tlsAcceptor, Encoding fallbackEncoding, DicomServiceOptions options, object userState);

        //
        // Resumen:
        //     Stop server from further listening.
        void Stop();
    }
}

// DicomServerFactory clase estatica 


namespace FellowOakDicom.Network
{
    public static class DicomServerFactory
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
        public static IDicomServer Create<T>(int port, ITlsAcceptor tlsAcceptor = null, Encoding fallbackEncoding = null, ILogger logger = null, object userState = null) where T : DicomService, IDicomServiceProvider
        {
            return ServiceProviderServiceExtensions.GetRequiredService<IDicomServerFactory>(Setup.ServiceProvider).Create<T>(port, tlsAcceptor, fallbackEncoding, logger, userState);
        }

        //
        // Resumen:
        //     Creates a DICOM server object out of DI-container.
        //
        // Parámetros:
        //   ipAddress:
        //     IP address(es) to listen to. Value
        //     null
        //     applies default, IPv4Any.
        //
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
        public static IDicomServer Create<T>(string ipAddress, int port, ITlsAcceptor tlsAcceptor = null, Encoding fallbackEncoding = null, ILogger logger = null, object userState = null) where T : DicomService, IDicomServiceProvider
        {
            return ServiceProviderServiceExtensions.GetRequiredService<IDicomServerFactory>(Setup.ServiceProvider).Create<T>(ipAddress, port, tlsAcceptor, fallbackEncoding, logger, userState);
        }

        //
        // Resumen:
        //     Creates a DICOM server object out of DI-container.
        //
        // Parámetros:
        //   ipAddress:
        //     IP address(es) to listen to. Value
        //     null
        //     applies default, IPv4Any.
        //
        //   port:
        //     Port to listen to.
        //
        //   userState:
        //     Optional optional parameters.
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
        //   TServer:
        //     Concrete DICOM server type to be returned.
        //
        // Devuelve:
        //     An instance of TServer, that starts listening for connections in the background.
        public static IDicomServer Create<T, TServer>(string ipAddress, int port, object userState = null, ITlsAcceptor tlsAcceptor = null, Encoding fallbackEncoding = null, ILogger logger = null) where T : DicomService, IDicomServiceProvider where TServer : IDicomServer<T>
        {
            return ServiceProviderServiceExtensions.GetRequiredService<IDicomServerFactory>(Setup.ServiceProvider).Create<T, TServer>(ipAddress, port, userState, tlsAcceptor, fallbackEncoding, logger);
        }
    }
}


// Comentarios de actualizacion de biblioteca fo-dicom


/*
34  #### 5.0.3 (2022-05-23)
   35  * **Breaking change**: subclasses of DicomService will have to pass an instance of DicomServiceDependencies along to the DicomService base constructor. This replaces the old LogManager / NetworkManager / TranscoderManager dependencies. (Implemented in the context of #1291)
   36: * **Breaking change**: subclasses of DicomServer will have to pass an instance of DicomServerDependencies along to the DicomServer base constructor. This replaces the old NetworkManager / LogManager dependencies. (Implemented in the context of #1291)
   37  * **Breaking change**: DicomClient no longer has a NetworkManager, LogManager or TranscoderManager, these are to be configured via dependency injection. (Implemented in the context of #1144)
   38  * Update to DICOM Standard 2022b
   ..
  109  * Use Color32 instead of System.Drawing.Color (#1140)
  110  * FrameGeometry is enhanced so that it also works for DX, CR or MG images. (#1138)
  111: * DicomServerFactory missed the method overload to pass the userState object
  112  * Private Creator UN tags are converted to LO (#1146)
  113  * Bug fix: Ensure timeout detection can never stop prematurely
  ...
  127    * *INetworkManager:* creates a listner, opens streams or checks connection status.
  128    * *IDicomClientFactory:* is responsible to return an `IDicomClient` instance. This may be a new instance everytime or a reused instance per host or whateever.
  129:   * *IDicomServerFactory:* creates server instances, manages available ports, etc.
  130  * If there is no DI container provided, fo-dicom creates its own internally. To configure it, call `new DicomSetupBuilder().RegisterServices(s => ...).Build();`
  131    There are extension methods for this DicomSetupBuilder like `.SkipValidation()` or `SetDicomServiceLogging(logDataPdus, log DimseDataset)`.
  132    The new interface `IServiceProviderHost` manages, if there is an internal ServiceProvider or if to use a given Asp.Net Service Provider.
  133: * DicomServer uses DI to create the instances of your `DicomService`. You can use constructor injection there.
  134  * Make server providers asynchronous
  135  * A new class `DicomClientOptions` holds all the options of a DicomClient to be passed instead of the huge list of parameters.
  136: * `DicomServerRegistration` manages the started servers per IP/Port.
  137  * Some little memory consumption emprovements in IByteBuffer classes.
  138  * new methods in `IByteBuffer` to directly manipulate/use the data instead of copying it around multiple times.
  ...
  152  * By default there is only `RawImageManager` implementation for `IImageManager`. To have `WinFormsImageManager` or `WPFImageManager` you have to reference the package *fo-dicom.Imaging.Desktop*. To use `ImageSharpImageManager` you have to reference *fo-dicom.Imaging.ImageSharp*.
  153  * There are only asynchronous server provider interfaces. All synchronous methods have been replaced by asynchronous.
  154: * Instances of `DicomClient` and `DicomServer` are not created directly, but via a `DicomClientFactory` or a `DicomServerFactory`.
  155    If you are in a "DI-Environment" like Asp.Net, then inject a `IDicomClientFactory` instance and use this to create a DicomClient. otherwise call `DicomClientFactory.CreateDicomClient(...)`.  This is a wrapper around accessing the internal DI container , getting the registered IDicomClientFactory and then calling this. So this is more overhead.
  156: * DicomServiceOptions cannot be passed as parameter to DicomServer constructor/factory any more, but the values of options have to be set to the created instance of DicomServer.
  157  * Classes `DicomFileReader`, `DicomReader`, `DicomReaderCallbackObserver` etc are now internal instead of public, because the resulting Datasets are wrong/inconsistent and need further changes. Therefore its usage is dangerous for users. (#823)
  158  * Removed obsolete methods/classes/properties

  */