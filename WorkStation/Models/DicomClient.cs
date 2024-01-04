using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using FellowOakDicom.Network.Client.EventArguments;
using FellowOakDicom.Network.Tls;
using System.Text;

namespace WorkStation.Models
{
    public class DicomClient : IDicomClient
    {
        public string Host => throw new NotImplementedException();

        public int Port => throw new NotImplementedException();

        public ITlsInitiator TlsInitiator => throw new NotImplementedException();

        public string CallingAe => throw new NotImplementedException();

        public string CalledAe => throw new NotImplementedException();

        public ILogger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DicomServiceOptions ServiceOptions => throw new NotImplementedException();

        public DicomClientOptions ClientOptions => throw new NotImplementedException();

        public List<DicomPresentationContext> AdditionalPresentationContexts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<DicomExtendedNegotiation> AdditionalExtendedNegotiations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool RequireSuccessfulUserIdentityNegotiation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DicomUserIdentityNegotiation UserIdentityNegotiation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Encoding FallbackEncoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DicomClientCStoreRequestHandler OnCStoreRequest { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DicomClientNEventReportRequestHandler OnNEventReportRequest { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsSendRequired => throw new NotImplementedException();

        public event EventHandler<AssociationAcceptedEventArgs> AssociationAccepted;
        public event EventHandler<AssociationRejectedEventArgs> AssociationRejected;
        public event EventHandler AssociationReleased;
        public event EventHandler<AssociationRequestTimedOutEventArgs> AssociationRequestTimedOut;
        public event EventHandler<StateChangedEventArgs> StateChanged;
        public event EventHandler<RequestTimedOutEventArgs> RequestTimedOut;

        public Task AddRequestAsync(DicomRequest dicomRequest)
        {
            throw new NotImplementedException();
        }

        public Task AddRequestsAsync(IEnumerable<DicomRequest> dicomRequests)
        {
            throw new NotImplementedException();
        }

        public void NegotiateAsyncOps(int invoked = 0, int performed = 0)
        {
            throw new NotImplementedException();
        }

        public void NegotiateUserIdentity(DicomUserIdentityNegotiation userIdentityNegotiation)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(CancellationToken cancellationToken = default, DicomClientCancellationMode cancellationMode = DicomClientCancellationMode.ImmediatelyReleaseAssociation)
        {
            throw new NotImplementedException();
        }
    }
}
