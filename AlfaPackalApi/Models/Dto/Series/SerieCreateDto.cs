using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Series
{
    public class SerieCreateDto
    {
        // ID of the parent study, link to Study
        [Required]
        public int? PACSStudyID { get; set; }
        // Description
        public string SeriesDescription { get; set; }
        // Instance UID  
        [Required, MaxLength(64)]
        public string SeriesInstanceUID { get; set; }
        public int? SeriesNumber { get; set; }
        // Start date and time of the series
        public DateTime? SeriesDateTime { get; set; }
        // Patient Position.
        [MaxLength(16)]
        public string? PatientPosition { get; set; }
        //** Load Section **//
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }
    }
}
