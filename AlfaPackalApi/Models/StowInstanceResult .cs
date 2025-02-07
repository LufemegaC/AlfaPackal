using System.ComponentModel;

namespace Api_PACsServer.Models
{
    public class StowInstanceResult
    /**/
    {
        /// <summary>
        /// The SOP Class UID of the stored instance (Tag 0008,0016).
        /// </summary>
        public string SOPClassUID { get; set; }

        /// <summary>
        /// The SOP Instance UID of the stored instance (Tag 0008,0018).
        /// </summary>
        public string SOPInstanceUID { get; set; }

        /// <summary>
        /// Failure reason code if the operation failed (Tag 0008,1197).
        /// </summary>
        public int? FailureReason { get; set; }

        /// <summary>
        /// Warning reason code if the operation completed with warnings (Tag 0008,1196).
        /// </summary>
        public int? WarningReason { get; set; }

        /// <summary>
        /// Error messa
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Creates a successful result for a stored instance.
        /// </summary>
        public static StowInstanceResult Success(string sopClassUID, string sopInstanceUID)
        {
            return new StowInstanceResult
            {
                SOPClassUID = sopClassUID,
                SOPInstanceUID = sopInstanceUID
            };
        }


        /// <summary>
        /// Creates a failed result with a specified failure reason.
        /// </summary>
        private static StowInstanceResult Failure(string sopClassUID, string sopInstanceUID, DicomFailureReason failureReason, string message)
        {
            return new StowInstanceResult
            {
                SOPClassUID = sopClassUID,
                SOPInstanceUID = sopInstanceUID,
                FailureReason = (int)failureReason,
                Message = message
            };
        }

        /// <summary>
        /// Creates a warning result with a specified warning reason.
        /// </summary>
        private static StowInstanceResult Warning(string sopClassUID, string sopInstanceUID, DicomWarningReason warningReason, string message)
        {
            return new StowInstanceResult
            {
                SOPClassUID = sopClassUID,
                SOPInstanceUID = sopInstanceUID,
                WarningReason = (int)warningReason,
                Message = message
            };
        }

        // Common failure results
        public static StowInstanceResult ProcessingFailure(string sopClassUID, string sopInstanceUID, string message)
        {
            return Failure(sopClassUID, sopInstanceUID, DicomFailureReason.ProcessingFailure, message);
        }

        public static StowInstanceResult CannotUnderstand(string sopClassUID, string sopInstanceUID, string message)
        {
            return Failure(sopClassUID, sopInstanceUID, DicomFailureReason.CannotUnderstand, message);
        }

        public static StowInstanceResult OutOfResources(string sopClassUID, string sopInstanceUID, string message)
        {
            return Failure(sopClassUID, sopInstanceUID, DicomFailureReason.OutOfResources, message);
        }

        public static StowInstanceResult DataSetDoesNotMatchSOPClassFail(string sopClassUID, string sopInstanceUID, string message)
        {
            return Failure(sopClassUID, sopInstanceUID, DicomFailureReason.DataSetDoesNotMatchSOPClass, message);
        }

        public static StowInstanceResult TransferSyntaxNotSupported(string sopClassUID, string sopInstanceUID, string message)
        {
            return Failure(sopClassUID, sopInstanceUID, DicomFailureReason.TransferSyntaxNotSupported, message);
        }

        public static StowInstanceResult SOPClassUIDNotSupported(string sopClassUID, string sopInstanceUID, string message)
        {
            return Failure(sopClassUID, sopInstanceUID, DicomFailureReason.SOPClassUIDNotSupported, message);
        }

        // Common warning results

        public static StowInstanceResult DataSetDoesNotMatchSOPClassWarning(string sopClassUID, string sopInstanceUID, string message)
        {
            return Warning(sopClassUID, sopInstanceUID, DicomWarningReason.DataSetDoesNotMatchSOPClass, message);
        }

        public static StowInstanceResult ElementsDiscarded(string sopClassUID, string sopInstanceUID, string message)
        {
            return Warning(sopClassUID, sopInstanceUID, DicomWarningReason.ElementsDiscarded, message);
        }

        public static StowInstanceResult CoercionDataElements(string sopClassUID, string sopInstanceUID, string message)
        {
            return Warning(sopClassUID, sopInstanceUID, DicomWarningReason.CoercionDataElements, message);
        }

        /// <summary>
        /// Enumeration of failure reasons based on DICOM standards.
        /// </summary>
        private enum DicomFailureReason
        // Update -> DicomWeb 18.3 2024d
        // I Store Instance Response Module 
        // Table I.2-2. Store Instances Response Failure Reason Values
        {
            // Was out of resources.
            [Description("Out of Resources (0xA700)")]
            OutOfResources = 42752,

            //The Instance does not conform to its specified SOP Class.
            [Description("Dataset does not match SOP Class (0xA900)")]
            DataSetDoesNotMatchSOPClass = 43264,

            // Cannot understand certain Data Elements.
            [Description("Cannot Understand (0xC000)")]
            CannotUnderstand = 49152,

            //Does not support the requested Transfer Syntax for the Instance
            [Description("Transfer Syntax Not Supported (0xC122)")]
            TransferSyntaxNotSupported = 49442,

            // General failure in processing the operation.(Default)
            [Description("Processing Failure (0x0110)")]
            ProcessingFailure = 272,

            //Does not support the requested SOP Class.
            [Description("SOP Class UID Not Supported (0x0122)")]
            SOPClassUIDNotSupported = 290
        }


        /// <summary>
        /// Enumeration of warning reasons based on DICOM standards.
        /// </summary>
        private enum DicomWarningReason
        // Update -> DicomWeb 18.3 2024d
        // I Store Instance Response Module 
        // Table I.2-1. Store Instances Response Warning Reason Values
        {
            // Modified one or more data elements during storage of the Instance
            [Description("Coercion of Data Elements (0xB000)")]
            CoercionDataElements = 45056,

            // Discarded some data elements during storage of the Instance
            [Description("Elements Discarded (0xB006)")]
            ElementsDiscarded = 45062,

            // Modified one or more data elements during storage of the Instance
            [Description("Coercion of Data Elements (0xB007)")]
            DataSetDoesNotMatchSOPClass = 45063

        }
    }
}
