using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Series
{
    public class SerieDto
    {
        // Description
        public string SeriesDescription { get; set; }
        // Instance UID  
        [Required, MaxLength(64)]
        public string SeriesInstanceUID { get; set; }
        // Series number
        public int? SeriesNumber { get; set; }
        // Start date and time of the series
        public DateTime? SeriesDateTime { get; set; }
        // Patient Position 
        public string? PatientPosition { get; set; }
        // Number of related instances
        public int? NumberOfInstances { get; set; }
        // Total size of the series in MB
        public decimal TotalFileSizeMB { get; set; }

    }
}
