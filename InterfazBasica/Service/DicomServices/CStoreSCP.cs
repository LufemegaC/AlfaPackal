using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Service.IService.Dicom;
using System.Text;


namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class CStoreSCP : DicomService, IDicomServiceProvider, IDicomCStoreProvider, IDicomCEchoProvider
    {
        // 25/01/24 Luis Felipe MG.-Dependencias
        private IDicomOrchestrator _dicomOrchestrator;

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
            _dicomOrchestrator = ServiceLocator.GetService<IDicomOrchestrator>();
        }

        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            var AEtitle = _dicomOrchestrator.GetServerAEtitle();
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
                // DicomFile apartir de DataSet
                DicomFile dicomFile = new DicomFile(request.Dataset);
                // Se entrega al orchestrator para registro de entidades PACS
                var resultStoreDicomData = await _dicomOrchestrator.StoreDicomData(dicomFile);
                //var resultStoreDicomFile = await _dicomOrchestrator.StoreDicomFile(dicomFile);

                // Envio de archivo DICOM para su almacenamiento fisico
                //var resultStoreDicomFile = _dicomOrchestrator.StoreDicomFile(dicomFile);
                //Si falla el proceso de almacenamiento
                return new DicomCStoreResponse(request, resultStoreDicomData);
            }
            catch 
            {
                return new DicomCStoreResponse(request, DicomStatus.Warning);
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
