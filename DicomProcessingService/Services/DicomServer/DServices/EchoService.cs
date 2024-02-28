using AutoMapper;
using DicomProcessingService.Services.Interfaces;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using FellowOakDicom.Network;
using System.Text;
using Utileria;

namespace DicomProcessingService.Services.DicomServer.DServices
{
    public class EchoService : DicomService, IDicomServiceProvider, IDicomCEchoProvider , IDicomCStoreProvider
    {

        private IDicomOrchestrator _dicomOrchestrator;
        //private readonly IMapper _mapper;
        private string _CallingAE { get; set; }
        private string _CalledAE { get; set; }
        // Funcion delegado para el evento
        //public event EventHandler<DicomAssociation> AssociationReceived;

        // Sintaxis de transferencia DICOM soportados por el servidor ( Datos generales )
        private static readonly DicomTransferSyntax[] _acceptedTransferSyntaxes = new DicomTransferSyntax[]
        {
            DicomTransferSyntax.ExplicitVRLittleEndian,
            DicomTransferSyntax.ExplicitVRBigEndian,
            DicomTransferSyntax.ImplicitVRLittleEndian
        };

        //Sintaxis de transferencia DICOM soportados por el servidor ( Datos de imagen )
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
        /* Constructor:
         INetworkStream: Facilita la lectura y escritura de datos en la red.
         Encoding: Define la codificación de caracteres a utilizar para interpretar los bytes de los mensajes DICOM.
         ILogger: Registrar información, advertencias y errores.
         DicomServiceDependencies: Dependencias opcionales para servicios DICOM.
         */
        public EchoService(INetworkStream stream, Encoding fallbackEncoding, ILogger log, DicomServiceDependencies dependencies)
                : base(stream, fallbackEncoding, log, dependencies)
        {
            /* initialization per association can be done here */
            //_dicomOrchestrator  = 
        }

        //// Método de inicialización para inyectar dependencias
        //public void Initialize()
        //{
        //    _dicomOrchestrator = Setup.ServiceProvider.GetRequiredService<IDicomOrchestrator>();
        //}


        //public EchoService(INetworkStream stream, Encoding fallbackEncoding, ILogger log, DicomServiceDependencies dependencies, IEstudioService estudioService, IMapper mapper)
        //: base(stream, fallbackEncoding, log, dependencies)
        //{
        //    _estudioService = estudioService;
        //    _mapper = mapper;
        //}

        public Task<DicomCEchoResponse> OnCEchoRequestAsync(DicomCEchoRequest request)
        {
            return Task.FromResult(new DicomCEchoResponse(request, DicomStatus.Success));
        }

        public void OnConnectionClosed(Exception exception)
        {
            Clean();
        }

        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
            Logger.LogError($"Received abort from {source}, reason is {reason}");
        }

        public Task OnReceiveAssociationReleaseRequestAsync()
        {
            Clean();
            return SendAssociationReleaseResponseAsync();
        }

        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            //Respaldo de informacion
            _CallingAE = association.CallingAE;
            _CalledAE = association.CalledAE;

            // Imprime un mensaje de log con la información de la solicitud de asociación
            Logger.LogInformation($"Received association request from AE: {_CallingAE} with IP: {association.RemoteHost} ");

            // Comprueba si el AE llamado coincide con el AE del servidor
            if (EchoServer.AETitle != _CalledAE)
            {
                // Imprime un mensaje de error y envía un rechazo de asociación si el AE llamado es desconocido
                Logger.LogError($"Association with {_CallingAE} rejected since called aet {_CalledAE} is unknown");
                return SendAssociationRejectAsync(DicomRejectResult.Permanent, DicomRejectSource.ServiceUser, DicomRejectReason.CalledAENotRecognized);
            }

            //Recorre los contextos de presentación en la asociación
            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification
                    || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelFind
                    || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelMove
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelFind
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelMove)
                {
                    // Si es así, acepta los sintaxis de transferencia
                    pc.AcceptTransferSyntaxes(_acceptedTransferSyntaxes);
                }
                else if (pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelGet
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelGet)
                {
                    // Acepta los sintaxis de transferencia de imagen si el contexto de presentación es para obtener información
                    pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
                }
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                {
                    // Acepta los sintaxis de transferencia de imagen si el contexto de presentación es para la categoría de almacenamiento que no es None
                    pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
                }
                else
                {
                    // Si el contexto de presentación no es soportado, registra una advertencia y establece el resultado a Rechazar
                    Logger.LogWarning($"Requested abstract syntax {pc.AbstractSyntax} from {_CallingAE} not supported");
                    //Logger.Warn($"Requested abstract syntax {pc.AbstractSyntax} from {CallingAE} not supported");
                    pc.SetResult(DicomPresentationContextResult.RejectAbstractSyntaxNotSupported);
                }
            }
            // Imprime un mensaje de log que la solicitud de asociación ha sido aceptada

            Logger.LogInformation($"Accepted association request from {_CallingAE}");
            // Ejecucion de evento


            // Envía una respuesta de aceptación de asociación
            return SendAssociationAcceptAsync(association);
        }
        //* C - STORE *//
        public async Task<DicomCStoreResponse> OnCStoreRequestAsync(DicomCStoreRequest request)
        {
            // Extraer UID de estudio e instancia

            try
            {
                // UIDs
                var studyUid = request.Dataset.GetSingleValue<string>(DicomTag.StudyInstanceUID).Trim();
                var instUid = request.SOPInstanceUID.UID;
                var seriesUid = request.Dataset.GetSingleValue<string>(DicomTag.SeriesInstanceUID).Trim();
                var sopClassUID = request.Dataset.GetSingleValue<string>(DicomTag.SOPClassUID);

                //Creacion de archivo DICOM
                DicomDataset dataset = request.Dataset;
                DicomFile dicomFile = new DicomFile(dataset);

                if (request.Dataset.Contains(DicomTag.PixelData))
                {
                    //var pixelData = request.Dataset.GetDicomItem<DicomPixelData>(DicomTag.PixelData);
                    var pixelData = DicomPixelData.Create(request.Dataset);
                    // Aquí puedes trabajar con pixelData, por ejemplo, acceder a los frames de la imagen
                    for (int i = 0; i < pixelData.NumberOfFrames; i++)
                    {
                        var frame = pixelData.GetFrame(i);
                        byte[] pixelBytes = frame.Data;
                        // Hacer algo con los bytes de píxeles
                    }
                }
                //Detonacion de eventos

                // Aqui Ixchel
                //EstudioCreateDto estudioDto = _mapper.Map<EstudioCreateDto>(request.Dataset);
                //var response = await _estudioService.Crear<APIResponse>(estudioDto);
                //if (response != null && response.IsExitoso)
                //{ 
                //    // Insetar Series
                //}
                var path = Path.Combine(Path.GetFullPath(DS.RutaAlmacen), studyUid, seriesUid);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = Path.Combine(path, instUid + ".dcm");

                await request.File.SaveAsync(path);

                return new DicomCStoreResponse(request, DicomStatus.Success);
            }
            catch (Exception ex)
            {
                return new DicomCStoreResponse(request, DicomStatus.Warning);
            }

        }


        public Task OnCStoreRequestExceptionAsync(string tempFileName, Exception e)
        {
            // let library handle logging and error response
            return Task.CompletedTask;
        }

        public void Clean()
        {
            // cleanup, like cancel outstanding move- or get-jobs
        }

    }
}

