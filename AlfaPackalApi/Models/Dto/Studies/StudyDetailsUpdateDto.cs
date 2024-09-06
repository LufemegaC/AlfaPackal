using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Studies
{
    public class StudyDetailsUpdateDto
    {
        // pk
        public string StudyInstanceUID { get; set; }
        // Number of DICOM files related to the study.
        public int? NumberOfStudyRelatedInstances { get; set; }
        // Number of series in the study.
        public int? NumberOfStudyRelatedSeries { get; set; }
        // Access number
        [MaxLength(64)]
        public string? AccessionNumber { get; set; }
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }
    }
}
