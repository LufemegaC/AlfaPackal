namespace Api_PACsServer.Models.Dto.Studies
{
    public class StudyLoadUpdateDto
    {
        // pk
        public int PACSStudyLoadID { get; set; }
        // Number of DICOM files related to the study.
        public int? NumberOfFiles { get; set; }
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }
        // Number of series in the study.
        public int? NumberOfSeries { get; set; }
    }
}
