using AlfaPackalApi.Modelos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Modelos.IModels;

namespace Api_PACsServer.Modelos.Load
{
    public class SerieDetails : IAuditable
    {
        // PK: 'SRD-'+StudyId+'-'+SeriesNumber+'-'+N
        [Key]
        public string SeriesInstanceUID { get; set; }
        // Foreign Key 
        [Required, ForeignKey("SeriesInstanceUID")]
        public virtual Serie Serie { get; set; }
        // Number of related instances
        public int? NumberOfSeriesRelatedInstances { get; set; }
        // Total size of the series in MB
        public decimal TotalFileSizeMB { get; set; }
        // Audit fields
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
