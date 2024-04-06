using FellowOakDicom;
using FellowOakDicom.Network;
using InterfazBasica_DCStore.Service.DicomServers;
using Microsoft.AspNetCore.Hosting.Server;
using System.Text;

namespace InterfazBasica_DCStore.Service.DicomServices
{
    public class CFindSCP : DicomService, IDicomCEchoProvider, IDicomServiceProvider, IDicomCFindProvider
    {
        // 

        //
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


        public string CallingAE { get; protected set; }
        public string CalledAE { get; protected set; }


        public CFindSCP(INetworkStream stream, Encoding fallbackEncoding, ILogger log, DicomServiceDependencies dependencies)
                : base(stream, fallbackEncoding, log, dependencies)
        {
            /* initialization per association can be done here */
        }

        public Task<DicomCEchoResponse> OnCEchoRequestAsync(DicomCEchoRequest request)
        {
            Logger.LogInformation($"Received verification request from AE {CallingAE} with IP: {Association.RemoteHost}");
            return Task.FromResult(new DicomCEchoResponse(request, DicomStatus.Success));
        }


        public void OnConnectionClosed(Exception exception)
        {
            Clean();
        }


        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
            //log the abort reason
            Logger.LogError($"Received abort from {source}, reason is {reason}");
        }


        public Task OnReceiveAssociationReleaseRequestAsync()
        {
            Clean();
            return SendAssociationReleaseResponseAsync();
        }


        public Task OnReceiveAssociationRequestAsync(DicomAssociation association)
        {
            CallingAE = association.CallingAE;
            CalledAE = association.CalledAE;

            Logger.LogInformation($"Received association request from AE: {CallingAE} with IP: {association.RemoteHost} ");

            //if (QRServer.AETitle != CalledAE)
            //{
            //    Logger.LogError($"Association with {CallingAE} rejected since called aet {CalledAE} is unknown");
            //    return SendAssociationRejectAsync(DicomRejectResult.Permanent, DicomRejectSource.ServiceUser, DicomRejectReason.CalledAENotRecognized);
            //}

            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification
                    || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelFind
                    || pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelMove
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelFind
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelMove)
                {
                    pc.AcceptTransferSyntaxes(_acceptedTransferSyntaxes);
                }
                else if (pc.AbstractSyntax == DicomUID.PatientRootQueryRetrieveInformationModelGet
                    || pc.AbstractSyntax == DicomUID.StudyRootQueryRetrieveInformationModelGet)
                {
                    pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
                }
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                {
                    pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
                }
                else
                {
                    Logger.LogWarning($"Requested abstract syntax {pc.AbstractSyntax} from {CallingAE} not supported");
                    pc.SetResult(DicomPresentationContextResult.RejectAbstractSyntaxNotSupported);
                }
            }

            Logger.LogInformation($"Accepted association request from {CallingAE}");
            return SendAssociationAcceptAsync(association);
        }


        public async IAsyncEnumerable<DicomCFindResponse> OnCFindRequestAsync(DicomCFindRequest request)
        {
            var queryLevel = request.Level;

            var matchingFiles = new List<string>();
            //IDicomImageFinderService finderService = ServerCFind.CreateFinderService;

            //// a QR SCP has to define in a DICOM Conformance Statement for which dicom tags it can query
            //// depending on the level of the query. Below there are only very few parameters evaluated.

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

            // now read the required dicomtags from the matching files and return as results
            foreach (var matchingFile in matchingFiles)
            {
                var dicomFile = DicomFile.Open(matchingFile);
                var result = new DicomDataset();
                foreach (var requestedTag in request.Dataset)
                {
                    // most of the requested DICOM tags are stored in the DICOM files and therefore saved into a database.
                    // you can fill the responses by selecting the values from the database.
                    // also be aware that there are some requested DicomTags like "ModalitiesInStudy" or "NumberOfStudyRelatedInstances" 
                    // or "NumberOfPatientRelatedInstances" and so on which have to be calculated and cannot be read from a DICOM file.
                    if (dicomFile.Dataset.Contains(requestedTag.Tag))
                    {
                        dicomFile.Dataset.CopyTo(result, requestedTag.Tag);
                    }
                    // else if (requestedTag == DicomTag.NumberOfStudyRelatedInstances)
                    // {
                    //    ... somehow calculate how many instances are stored within the study
                    //    result.Add(DicomTag.NumberOfStudyRelatedInstances, number);
                    // } ....
                    else
                    {
                        result.Add(requestedTag);
                    }
                }
                yield return new DicomCFindResponse(request, DicomStatus.Pending) { Dataset = result };
            }

            yield return new DicomCFindResponse(request, DicomStatus.Success);
        }


        public void Clean()
        {
            // cleanup, like cancel outstanding move- or get-jobs
        }


    }
}
