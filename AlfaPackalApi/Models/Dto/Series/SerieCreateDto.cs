using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Series
{
    public class SerieCreateDto
    {
        // ID of the parent study, link to Study
        [Required]
        public string SeriesInstanceUID { get; set; }
        [Required]
        public string StudyInstanceUID { get; set; }
        // Description
        // Series number
        public int SeriesNumber { get; set; }
        public string? SeriesDescription { get; set; }

        // Study modality DICOM/Metadata
        public string? Modality { get; set; }
        // Start date and time of the series
        public DateTime? SeriesDateTime { get; set; }
        // Body Part Examined
        public string? BodyPartExamined { get; set; }
        // Patient Position.
        [MaxLength(16)]
        public string? PatientPosition { get; set; }
        public string? ProtocolName { get; set; }
        public string? SeriesDate { get; set; }
        public string? SeriesTime { get; set; }
        //** Details Section **//
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }
    }
}
