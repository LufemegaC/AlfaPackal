using FellowOakDicom;
using System.Threading.Channels;

namespace InterfazBasica_DCStore.Utilities
{
    public class DicomChannelSingleton
    {
        // Bounded or unbounded, depende de si quieres un límite
        public static readonly Channel<DicomFile> DicomChannel = Channel.CreateUnbounded<DicomFile>();

    }
}
