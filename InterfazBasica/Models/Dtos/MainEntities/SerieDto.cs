using System.ComponentModel.DataAnnotations;

namespace InterfazBasica_DCStore.Models.Dtos.MainEntities
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
        // Study modality DICOM/Metadata
        public string? Modality { get; set; }
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
