using DicomProcessingService.Services.DicomServer.DServices;
using DicomProcessingService.Services.Interfaces;
using FellowOakDicom;
using FellowOakDicom.Network;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DicomProcessingService.Services.DicomServer
{
    public class EchoServer
    {
        private static IDicomServer _server;
        private static EchoService _echoservice;
        
        private static int _port; // Puerto (104)
        public static string AETitle;// AEt
        private static IDicomOrchestrator _dicomOrchestrator;

        /*
        // Campos privados para almacenar las rutas y archivos necesarios para la prueba.
        private string _rootPath; // Ruta del directorio raíz donde se ejecuta la prueba.
        private DicomFile _sampleFile; // Archivo DICOM que se utiliza como muestra para las pruebas. 
        // Campos privados para los servidores y fábricas de servidores y clientes DICOM.
        private IDicomServer _cStoreServer; // Servidor para pruebas C-STORE.
        private IDicomServerFactory _dicomServerFactory; // Fábrica para crear instancias de servidores DICOM.
        private IDicomClientFactory _dicomClientFactory; // Fábrica para crear instancias de clientes DICOM.
        private IDicomServer _cEchoServer; // Servidor para pruebas C-ECHO.
        private IDicomClient _cEchoClient; // Cliente DICOM para enviar solicitudes C-ECHO.
        private IDicomClient _cStoreClient; // Cliente DICOM para enviar solicitudes C-STORE.
        */

        //public static IDicomImageFinderService CreateFinderService => new StupidSlowFinderService();
        //public static IDicomImageFinderService CreateFinderService { get; private set; } = new StupidSlowFinderService();

        public EchoServer(IDicomOrchestrator dicomOrchestrator)
        {
            _dicomOrchestrator = dicomOrchestrator;
        }
        
        
        
        public static bool IsRunning => _server?.IsListening ?? false; //Bandera, Estatus del server
        public static string IPAddress => _server?.IPAddress ?? "Sin Asignar";
        public static void Start(int port, string aet)
        {
            //Logger.Info($"Servidor arriba AE: {aet}");
            Stop(); //Detengo servidor
            _port = port; //Puerto
            AETitle = aet; //AETitle
            _server = DicomServerFactory.Create<EchoService>(port); //Levanta servidor
            //_server.StartAsync()
            //_echoservice = DicomServerFactory.Create<EchoService>(port, userState: _dicomOrchestrator); //Levanta servidor
            //_echoservice = DicomServerFactory.Create<EchoService>(port)
            //_echoservice.Initialize(Setup.ServiceProvider.GetService<IDicomOrchestrator>());

            // Aquí resuelves la dependencia del contenedor de servicios
            //var dicomOrchestrator = setupBuilder.GetRequiredService<IDicomOrchestrator>();

            // Aquí resuelves la dependencia del contenedor de servicios

            //var dicomOrchestrator = serviceProvider.GetRequiredService<IDicomOrchestrator>();

            //_server.Initialize(Setup .ServiceProvider.GetService<IDicomOrchestrator>());
            //_server = DicomServerFactory.Create<EchoService>(port, null, null, null, serviceProvider);

            //_server.Initialize(_dicomOrchestrator);
            //_server.AssociationReceived += HandleCStoreRequestAsync;

        }


        /*
         public void Setup()
        {
            // Configuración del contenedor de servicios para inyectar dependencias.
            var services = new ServiceCollection();

            services.AddFellowOakDicom() // Añade los servicios DICOM de Fellow Oak.
                .Configure<DicomClientOptions>(o =>
                {
                    o.AssociationLingerTimeoutInMs = 0; // Tiempo de espera antes de cerrar la asociación DICOM.
                })
                .Configure<DicomServiceOptions>(o =>
                {
                    o.LogDataPDUs = false; // Desactiva el registro de Unidades de Datos de Protocolo.
                    o.LogDimseDatasets = false; // Desactiva el registro de conjuntos de datos DIMSE.
                    o.MaxPDULength = 512 * 1024 * 1024; // Establece la longitud máxima de PDU.
                })
                .AddSingleton<ILoggerFactory, NullLoggerFactory>(); // Añade una fábrica de loggers que no hace nada.

             // Construye el proveedor de servicios a partir de la colección de servicios.
            var serviceProvider = services.BuildServiceProvider();
            // Obtiene las fábricas necesarias del proveedor de servicios.
            _dicomServerFactory = serviceProvider.GetRequiredService<IDicomServerFactory>();
            _dicomClientFactory = serviceProvider.GetRequiredService<IDicomClientFactory>();
            
            // Establece las rutas y archivos necesarios para la prueba.
            _rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // Obtiene la ruta del ensamblado actual.
            _sampleFile = DicomFile.Open(Path.Combine(_rootPath, "Data\\GH355.dcm")); // Abre un archivo DICOM de muestra.
            // Creación de un servidor DICOM para operaciones C-STORE en el puerto 11118.
            _cStoreServer = _dicomServerFactory.Create<NopCStoreProvider>(11118);
            // Creación de un servidor DICOM para operaciones C-ECHO en el puerto 11119.
            _cEchoServer = _dicomServerFactory.Create<DicomCEchoProvider>(11119);
            // Creación de un cliente DICOM para comunicarse con el servidor C-ECHO.
            _cEchoClient = _dicomClientFactory.Create("127.0.0.1", _cEchoServer.Port, false, "SCU", "ANY-SCP");
            // Configuración del cliente C-ECHO para no registrar datasets DIMSE ni PDUs.
            _cEchoClient.ServiceOptions.LogDimseDatasets = false;
            _cEchoClient.ServiceOptions.LogDataPDUs = false;
            // Configuración de opciones del cliente C-ECHO: tiempo de espera y número máximo de solicitudes por asociación.
            _cEchoClient.ClientOptions.AssociationLingerTimeoutInMs = 0;
            _cEchoClient.ClientOptions.MaximumNumberOfRequestsPerAssociation = 1;
            // Creación de un cliente DICOM para comunicarse con el servidor C-STORE.
            _cStoreClient = _dicomClientFactory.Create("127.0.0.1", _cStoreServer.Port, false, "SCU", "ANY-SCP");
            // Configuración del cliente C-STORE similar a la del cliente C-ECHO.
            _cStoreClient.ServiceOptions.LogDimseDatasets = false;
            _cStoreClient.ServiceOptions.LogDataPDUs = false;
            _cStoreClient.ClientOptions.AssociationLingerTimeoutInMs = 0;
            _cStoreClient.ClientOptions.MaximumNumberOfRequestsPerAssociation = 1;
        }
         
        */
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
