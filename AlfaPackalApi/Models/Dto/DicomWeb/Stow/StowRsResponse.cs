using Newtonsoft.Json;

namespace Api_PACsServer.Models.Dto.DicomWeb.Stow
{
    /// <summary>
    /// Represents the response structure for a STOW-RS request.
    /// This class contains transaction metadata and status of stored instances.
    /// </summary>
    /// <author> Luis Felipe MG </author>
    /// <date> 2025-01-31 </date>
    public class StowRsResponse
    {
        /// <summary>
        /// Transaction UID (Tag 0008,1195)
        /// </summary>
        [JsonProperty("00081195")]
        public string TransactionUID { get; private set; }

        /// <summary>
        /// Retrieve URL (Tag 0008,1190)
        /// </summary>
        [JsonProperty("00081190")]
        public string RetrieveURL { get; private set; }

        /// <summary>
        /// List of successfully stored SOP instances (Tag 0008,1199)
        /// </summary>
        [JsonProperty("00081199")]
        public List<ReferencedSOPInstance> ReferencedSOPSequence { get; private set; }

        /// <summary>
        /// List of failed SOP instances (Tag 0008,1198)
        /// </summary>
        [JsonProperty("00081198")]
        public List<FailedInstance> FailedSOPSequence { get; private set; }

        /// <summary>
        /// Private constructor to enforce the use of the factory method.
        /// </summary>
        private StowRsResponse(string transactionUID, string retrieveURL,
            List<ReferencedSOPInstance> referencedSOPSequence,
            List<FailedInstance> failedSOPSequence)
        {
            TransactionUID = transactionUID ?? throw new ArgumentNullException(nameof(transactionUID));
            RetrieveURL = retrieveURL ?? throw new ArgumentNullException(nameof(retrieveURL));
            ReferencedSOPSequence = referencedSOPSequence ?? new List<ReferencedSOPInstance>();
            FailedSOPSequence = failedSOPSequence ?? new List<FailedInstance>();
        }

        /// <summary>
        /// Creates an instance of StowRsResponse from a list of StowInstanceResults.
        /// </summary>
        /// <param name="results">List of STOW instance results.</param>
        /// <param name="transactionUID">Unique identifier for the transaction.</param>
        /// <param name="retrieveURL">URL for retrieving stored instances.</param>
        /// <returns>An instance of StowRsResponse.</returns>
        public static StowRsResponse FromStowInstanceResults(
            List<StowInstanceResult> results, string transactionUID, string retrieveURL)
        {
            if (results == null || !results.Any())
                throw new ArgumentException("No instance results provided for STOW-RS response.");

            if (string.IsNullOrEmpty(transactionUID))
                throw new ArgumentException("Transaction UID cannot be null or empty.");

            if (string.IsNullOrEmpty(retrieveURL))
                throw new ArgumentException("Retrieve URL cannot be null or empty.");

            var referencedSOPSequence = results
                .Where(r => r.FailureReason == null)
                .Select(ReferencedSOPInstance.FromStowInstanceResult)
                .ToList();

            var failedSOPSequence = results
                .Where(r => r.FailureReason != null)
                .Select(FailedInstance.FromStowInstanceResult)
                .ToList();

            return new StowRsResponse(transactionUID, retrieveURL, referencedSOPSequence, failedSOPSequence);
        }
    }
}
}
