using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Series
{
    public class SerieDetailsCreateDto
    {
        public SerieDetailsCreateDto(string instanceUID,decimal totalSizeFile)
        {
            SeriesInstanceUID = instanceUID;
            NumberOfSeriesRelatedInstances = 1;
            TotalFileSizeMB = totalSizeFile;
            CreationDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }
        // PK: 'SRD-'+StudyId+'-'+SeriesNumber+'-'+N
        [Required]
        public string SeriesInstanceUID { get; set; }
        // Number of related instances
        public int? NumberOfSeriesRelatedInstances { get; set; }
        // Total size of the series in MB
        public decimal TotalFileSizeMB { get; set; }
        // Audit fields
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
