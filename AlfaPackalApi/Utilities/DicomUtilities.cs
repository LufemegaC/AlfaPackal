using FellowOakDicom;

namespace Api_PACsServer.Utilities
{
    public class DicomUtilities
    {
        public static bool ValidateUID(string uid)
        {
            return DicomUID.IsValidUid(uid);
        }
    }
}
