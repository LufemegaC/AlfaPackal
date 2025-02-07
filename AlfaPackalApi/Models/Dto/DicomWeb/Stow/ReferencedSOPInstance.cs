using Newtonsoft.Json;

namespace Api_PACsServer.Models.Dto.DicomWeb.Stow
{
    /// <summary>
    /// Represents a referenced SOP instance in a STOW-RS request.
    /// This class contains information about successfully stored instances.
    /// </summary>
    /// <author> Luis Felipe MG </author>
    /// <date> 2025-01-31 </date>
    public class ReferencedSOPInstance
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
        /// Warning Reason (Tag 0008,1196)
        /// </summary>
        [JsonProperty("00081196")]
        public int? WarningReason { get; private set; }

        /// <summary>
        /// Private constructor to enforce the use of the factory method.
        /// </summary>
        private ReferencedSOPInstance(string sopClassUID, string sopInstanceUID, int? warningReason)
        {
            SOPClassUID = sopClassUID ?? throw new ArgumentNullException(nameof(sopClassUID));
            SOPInstanceUID = sopInstanceUID ?? throw new ArgumentNullException(nameof(sopInstanceUID));
            WarningReason = warningReason;
        }

        /// <summary>
        /// Creates a ReferencedSOPInstance from a StowInstanceResult.
        /// </summary>
        /// <param name="result">Instance result to convert.</param>
        /// <returns>New ReferencedSOPInstance.</returns>
        public static ReferencedSOPInstance FromStowInstanceResult(StowInstanceResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            return new ReferencedSOPInstance(result.SOPClassUID, result.SOPInstanceUID, result.WarningReason);
        }
    }
}
