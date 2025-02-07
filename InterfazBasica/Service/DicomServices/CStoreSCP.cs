using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Service.IService.Dicom;
using InterfazBasica_DCStore.Utilities;
using System.Text;


namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class CStoreSCP : DicomService, IDicomServiceProvider, IDicomCStoreProvider, IDicomCEchoProvider
    {
        // Service for validation process
        private readonly IDicomValidationService _validationService;
        private readonly ILogger<CStoreSCP> _logger;


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
        //** Constructor original

        public CStoreSCP(INetworkStream stream, Encoding fallbackEncoding, ILogger log, DicomServiceDependencies dependencies)
            : base(stream, fallbackEncoding, log, dependencies)
        {
            _validationService = ServiceLocator.GetService<IDicomValidationService>();
            _logger = ServiceLocator.GetService<ILogger<CStoreSCP>>();
        }

        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            var AEtitle = LocalUtility.Aetitle;
            if (association.CalledAE != AEtitle) 
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
            try
            {
                // 1) Crear el DicomFile
                var dicomFile = new DicomFile(request.Dataset);

                // 2) Validar
                var dicomStatus = _validationService.IsValidDicomFile(dicomFile);
                if (dicomStatus != DicomStatus.Success)
                {
                    // Manejo de error: si la validación falla, opcionalmente guardas en disco “Failed”, etc.
                    _logger.LogWarning("DICOM validation failed. Status: {0}", dicomStatus);
                    return new DicomCStoreResponse(request, dicomStatus);
                }

                // 3) (Opcional) Guardar en disco temporal antes de procesar
                //    await StoreDicomOnlocalDisk(dicomFile, StorageLocation.Temporary);

                // 4) Escribir en el Channel
                await DicomChannelSingleton.DicomChannel.Writer.WriteAsync(dicomFile);

                // 5) Devolver respuesta
                return new DicomCStoreResponse(request, DicomStatus.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en OnCStoreRequestAsync");
                return new DicomCStoreResponse(request, DicomStatus.ProcessingFailure);
            }
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


    }
}
