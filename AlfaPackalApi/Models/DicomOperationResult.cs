namespace Api_PACsServer.Models
{
    public class DicomOperationResult
    {

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        // DICOM UIDs
        public string StudyInstanceUID { get; set; }
        public string SeriesInstanceUID { get; set; }
        public string SOPInstanceUID { get; set; }

        // DICOM Failure Reason Code
        public int? FailureReason { get; set; } // Use nullable int to handle cases where there is no failure

        public static DicomOperationResult Success(string studyInstanceUID, string seriesInstanceUID, string sopInstanceUID)
        {
            return new DicomOperationResult
            {
                IsSuccess = true,
                StudyInstanceUID = studyInstanceUID,
                SeriesInstanceUID = seriesInstanceUID,
                SOPInstanceUID = sopInstanceUID
            };
        }

        public static DicomOperationResult Failure(string errorMessage, string studyInstanceUID, string seriesInstanceUID, string sopInstanceUID, int failureReason)
        {
            return new DicomOperationResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StudyInstanceUID = studyInstanceUID,
                SeriesInstanceUID = seriesInstanceUID,
                SOPInstanceUID = sopInstanceUID,
                FailureReason = failureReason
            };
        }
    }
}
