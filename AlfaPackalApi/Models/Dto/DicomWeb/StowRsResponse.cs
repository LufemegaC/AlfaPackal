using Newtonsoft.Json;

namespace Api_PACsServer.Models.Dto.DicomWeb
{
    public class StowRsResponse
    {
        // Constructor that accepts acceptedInstances and failedInstances
        public StowRsResponse(List<AcceptedInstance> acceptedInstances, List<FailedInstance> failedInstances)
        {
            AcceptedSOPSequence = acceptedInstances.Any() ? acceptedInstances : null;
            FailedSOPSequence = failedInstances.Any() ? failedInstances : null;
        }
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
