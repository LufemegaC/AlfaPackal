using FellowOakDicom;
using static InterfazBasica_DCStore.Utilities.DicomUtility;

namespace InterfazBasica_DCStore.Models.Dicom
{
    /// <summary>
    /// Represents a batch of DICOM files with associated metadata and processing logic.
    /// </summary>
    public class DicomBatch
    {
        /// <summary>
        /// Total size of the batch in megabytes (MB).
        /// </summary>
        public decimal TotalSizeMB { get; set; }

        /// <summary>
        /// List of DICOM files included in the batch.
        /// </summary>
        public List<DicomFile> Instances { get; private set; }

        /// <summary>
        /// StudyUID of the batch if it is homogeneous. Null for heterogeneous batches.
        /// </summary>
        public string StudyUID { get; private set; }

        /// <summary>
        /// Default constructor if needed for special cases (like empty batch creation).
        /// </summary>
        public DicomBatch()
        {
            Instances = new List<DicomFile>();
            TotalSizeMB = 0m;
            StudyUID = "NO_STUDY";
        }
    }

}
