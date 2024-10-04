using Newtonsoft.Json;

namespace Api_PACsServer.Models.Dto.DicomWeb
{
    public class AcceptedInstance
    {
        // Constructor that accepts a DicomOperationResult
        public AcceptedInstance(DicomOperationResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            StudyInstanceUID = result.StudyInstanceUID;
            SeriesInstanceUID = result.SeriesInstanceUID;
            SOPInstanceUID = result.SOPInstanceUID;
        }

        [JsonProperty("StudyInstanceUID")]
        public string StudyInstanceUID { get; set; }

        [JsonProperty("SeriesInstanceUID")]
        public string SeriesInstanceUID { get; set; }

        [JsonProperty("SOPInstanceUID")]
        public string SOPInstanceUID { get; set; }
    }
}
