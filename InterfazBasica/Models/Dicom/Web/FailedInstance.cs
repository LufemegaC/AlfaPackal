using Newtonsoft.Json;

namespace InterfazBasica_DCStore.Models.Dicom.Web
{
    public class FailedInstance
    {
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
