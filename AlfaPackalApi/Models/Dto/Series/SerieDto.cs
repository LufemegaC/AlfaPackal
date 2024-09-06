using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Series
{
    public class SerieDto
    {
        [MaxLength(64)]
        public string SeriesInstanceUID { get; set; }
        public string StudyInstanceUID { get; set; }
        // Description
        public string SeriesDescription { get; set; }
        // Instance UID  
        // Series number
        public int? SeriesNumber { get; set; }
        // Study modality DICOM/Metadata
        public string? Modality { get; set; }
        // Start date and time of the series
        public DateTime? SeriesDateTime { get; set; }
        // Patient Position 
        public string? PatientPosition { get; set; }
        // Number of related instances
        public int? NumberOfSeriesRelatedInstances { get; set; }
        // Total size of the series in MB
        public decimal TotalFileSizeMB { get; set; }

    }
}
