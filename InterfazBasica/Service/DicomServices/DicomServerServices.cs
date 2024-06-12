using FellowOakDicom.Network;
using FellowOakDicom;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using InterfazBasica_DCStore.Service.DicomServers;
using InterfazBasica_DCStore.Service.IDicomService;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class DicomServerServices : DicomService, IDicomServiceProvider, IDicomCStoreProvider, IDicomCEchoProvider, IDicomCFindProvider
    { 
        // 25/01/24 Luis Fe
        private IDicomOrchestrator _dicomOrchestrator;

        private DicomFile _dicomFile;

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

        public DicomServerServices(INetworkStream stream, Encoding fallbackEncoding, ILogger log, DicomServiceDependencies dependencies)
            : base(stream, fallbackEncoding, log, dependencies)
        {
            _dicomOrchestrator = ServiceLocator.GetService<IDicomOrchestrator>();
        }

        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            // Comenmtado temporalemten

            //CallingAE = association.CallingAE;
            //CalledAE = association.CalledAE;

            //Logger.LogInformation($"Received association request from AE: {CallingAE} with IP: {association.RemoteHost} ");

            //if (QRServer.AETitle != CalledAE)
            //{
            //    Logger.LogError($"Association with {CallingAE} rejected since called aet {CalledAE} is unknown");
            //    return SendAssociationRejectAsync(DicomRejectResult.Permanent, DicomRejectSource.ServiceUser, DicomRejectReason.CalledAENotRecognized);
            //}

            //foreach (var pc in association.PresentationContexts)
            //{
            //    if (pc.AbstractSyntax == DicomUID.Verification
            //        || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelFind
            //        || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelMove
            //        || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelFind
            //        || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelMove)
            //    {
            //        pc.AcceptTransferSyntaxes(_acceptedTransferSyntaxes);
            //    }
            //    else if (pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelGet
            //        || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelGet)
            //    {
            //        pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
            //    }
            //    else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
            //    {
            //        pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
            //    }
            //    else
            //    {
            //        Logger.LogWarning($"Requested abstract syntax {pc.AbstractSyntax} from {CallingAE} not supported");
            //        pc.SetResult(DicomPresentationContextResult.RejectAbstractSyntaxNotSupported);
            //    }
            //}

            //Logger.LogInformation($"Accepted association request from {CallingAE}");
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

        public async IAsyncEnumerable<DicomCFindResponse> OnCFindRequestAsync(DicomCFindRequest request)
        {
            //var queryLevel = request.Level;

            //var matchingFiles = new List<string>();

            //IDicomImageFinderService finderService = ServerCFind.CreateFinderService;

            // a QR SCP has to define in a DICOM Conformance Statement for which dicom tags it can query
            // depending on the level of the query. Below there are only very few parameters evaluated.

            //switch (queryLevel)
            //{
            //    case DicomQueryRetrieveLevel.Patient:
            //        {
            //            var patname = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientName, string.Empty);
            //            var patid = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientID, string.Empty);

            //            matchingFiles = finderService.FindPatientFiles(patname, patid);
            //        }
            //        break;

            //    case DicomQueryRetrieveLevel.Study:
            //        {
            //            var patname = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientName, string.Empty);
            //            var patid = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientID, string.Empty);
            //            var accNr = request.Dataset.GetSingleValueOrDefault(DicomTag.AccessionNumber, string.Empty);
            //            var studyUID = request.Dataset.GetSingleValueOrDefault(DicomTag.StudyInstanceUID, string.Empty);

            //            matchingFiles = finderService.FindStudyFiles(patname, patid, accNr, studyUID);
            //        }
            //        break;

            //    case DicomQueryRetrieveLevel.Series:
            //        {
            //            var patname = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientName, string.Empty);
            //            var patid = request.Dataset.GetSingleValueOrDefault(DicomTag.PatientID, string.Empty);
            //            var accNr = request.Dataset.GetSingleValueOrDefault(DicomTag.AccessionNumber, string.Empty);
            //            var studyUID = request.Dataset.GetSingleValueOrDefault(DicomTag.StudyInstanceUID, string.Empty);
            //            var seriesUID = request.Dataset.GetSingleValueOrDefault(DicomTag.SeriesInstanceUID, string.Empty);
            //            var modality = request.Dataset.GetSingleValueOrDefault(DicomTag.Modality, string.Empty);

            //            matchingFiles = finderService.FindSeriesFiles(patname, patid, accNr, studyUID, seriesUID, modality);
            //        }
            //        break;

            //    case DicomQueryRetrieveLevel.Image:
            //        yield return new DicomCFindResponse(request, DicomStatus.QueryRetrieveUnableToProcess);
            //        yield break;
            //}

            //// now read the required dicomtags from the matching files and return as results
            //foreach (var matchingFile in matchingFiles)
            //{
            //    var dicomFile = DicomFile.Open(matchingFile);
            //    var result = new DicomDataset();
            //    foreach (var requestedTag in request.Dataset)
            //    {
            //        // most of the requested DICOM tags are stored in the DICOM files and therefore saved into a database.
            //        // you can fill the responses by selecting the values from the database.
            //        // also be aware that there are some requested DicomTags like "ModalitiesInStudy" or "NumberOfStudyRelatedInstances" 
            //        // or "NumberOfPatientRelatedInstances" and so on which have to be calculated and cannot be read from a DICOM file.
            //        if (dicomFile.Dataset.Contains(requestedTag.Tag))
            //        {
            //            dicomFile.Dataset.CopyTo(result, requestedTag.Tag);
            //        }
            //        // else if (requestedTag == DicomTag.NumberOfStudyRelatedInstances)
            //        // {
            //        //    ... somehow calculate how many instances are stored within the study
            //        //    result.Add(DicomTag.NumberOfStudyRelatedInstances, number);
            //        // } ....
            //        else
            //        {
            //            result.Add(requestedTag);
            //        }
            //    }
            //    yield return new DicomCFindResponse(request, DicomStatus.Pending) { Dataset = result };
            //}

            yield return new DicomCFindResponse(request, DicomStatus.Success);
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
