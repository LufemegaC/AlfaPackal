using Newtonsoft.Json;

namespace InterfazBasica_DCStore.Models.Dicom.Web
{
    public class StowRsResponse 
    {
        [JsonProperty("TransactionUID")]
        public string TransactionUID { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("AcceptedSOPSequence")]
        public List<AcceptedInstance> AcceptedSOPSequence { get; set; }

        [JsonProperty("FailedSOPSequence")]
        public List<FailedInstance> FailedSOPSequence { get; set; }
    }
}
