using FellowOakDicom.Network;

namespace Api_PACsServer.Utilities
{
    public static class QueryLevelHelper
    {
        public static readonly DicomQueryRetrieveLevel Patient = DicomQueryRetrieveLevel.Patient;
        public static readonly DicomQueryRetrieveLevel Study = DicomQueryRetrieveLevel.Study;
        public static readonly DicomQueryRetrieveLevel Series = DicomQueryRetrieveLevel.Series;
        public static readonly DicomQueryRetrieveLevel Image = DicomQueryRetrieveLevel.Image;
        public static readonly DicomQueryRetrieveLevel NotApplicable = DicomQueryRetrieveLevel.NotApplicable;
    }
}
