using Newtonsoft.Json;

namespace InterfazBasica_DCStore.Models.Dicom.Web
{
    public class AcceptedInstance
    {
        [JsonProperty("StudyInstanceUID")]
        public string StudyInstanceUID { get; set; }

        [JsonProperty("SeriesInstanceUID")]
        public string SeriesInstanceUID { get; set; }

        [JsonProperty("SOPInstanceUID")]
        public string SOPInstanceUID { get; set; }
    }
}
