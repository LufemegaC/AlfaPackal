using Newtonsoft.Json;

namespace Api_PACsServer.Models.Dto.DicomWeb
{
    public class FailedInstance
    {
        public FailedInstance(DicomOperationResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            StudyInstanceUID = result.StudyInstanceUID;
            SeriesInstanceUID = result.SeriesInstanceUID;
            SOPInstanceUID = result.SOPInstanceUID;
            FailureReason = result.FailureReason ?? 272;
        }
        [JsonProperty("StudyInstanceUID")]
        public string StudyInstanceUID { get; set; }

        [JsonProperty("SeriesInstanceUID")]
        public string SeriesInstanceUID { get; set; }

        [JsonProperty("SOPInstanceUID")]
        public string SOPInstanceUID { get; set; }

        [JsonProperty("FailureReason")]
        public int FailureReason { get; set; } // DICOM standard failure codes
    }
}
