using Newtonsoft.Json;

namespace Api_PACsServer.Models.Dto.DicomWeb.Stow
{
    /// <summary>
    /// Represents a failed SOP instance in a STOW-RS request.
    /// This class contains information about instances that failed to store.
    /// </summary>
    /// <author> Luis Felipe MG </author>
    /// <date> 2025-01-31 </date>
    public class FailedInstance
    {
        /// <summary>
        /// SOP Class UID (Tag 0008,0016)
        /// </summary>
        [JsonProperty("00080016")]
        public string SOPClassUID { get; private set; }

        /// <summary>
        /// SOP Instance UID (Tag 0008,0018)
        /// </summary>
        [JsonProperty("00080018")]
        public string SOPInstanceUID { get; private set; }

        /// <summary>
        /// Failure Reason (Tag 0008,1197)
        /// </summary>
        [JsonProperty("00081197")]
        public int FailureReason { get; private set; }

        /// <summary>
        /// Private constructor to enforce the use of the factory method.
        /// </summary>
        private FailedInstance(string sopClassUID, string sopInstanceUID, int failureReason)
        {
            SOPClassUID = sopClassUID ?? throw new ArgumentNullException(nameof(sopClassUID));
            SOPInstanceUID = sopInstanceUID ?? throw new ArgumentNullException(nameof(sopInstanceUID));
            FailureReason = failureReason;
        }

        /// <summary>
        /// Creates a FailedInstance from a StowInstanceResult.
        /// </summary>
        /// <param name="result">Instance result to convert.</param>
        /// <returns>New FailedInstance.</returns>
        public static FailedInstance FromStowInstanceResult(StowInstanceResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            if (result.FailureReason == null)
                throw new InvalidOperationException("FailureReason cannot be null for a failed instance.");

            return new FailedInstance(result.SOPClassUID, result.SOPInstanceUID, result.FailureReason.Value);
        }
    }
}
