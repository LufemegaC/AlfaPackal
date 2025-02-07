using InterfazBasica_DCStore.Models.Dicom;
using System.Threading.Channels;

namespace InterfazBasica_DCStore.Utilities
{
    /// <summary>
    /// Singleton channel for completed DicomBatch elements.
    /// We'll write DicomBatch objects here once they are ready to be processed by the next pipeline stage.
    /// </summary>
    public class DicomBatchChannelSingleton
    {
        // Unbounded channel for sending out the "completed" batches
        public static readonly Channel<DicomBatch> DicomBatchChannel = Channel.CreateUnbounded<DicomBatch>();
    }
}
