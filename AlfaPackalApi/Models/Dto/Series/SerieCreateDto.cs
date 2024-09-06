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
        public string SeriesDescription { get; set; }
        // Series number
        public int SeriesNumber { get; set; }  
        // Study modality DICOM/Metadata
        public string? Modality { get; set; }
        // Start date and time of the series
        public DateTime? SeriesDateTime { get; set; }
        // Patient Position.
        [MaxLength(16)]
        public string? PatientPosition { get; set; }
        //** Details Section **//
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }
    }
}
