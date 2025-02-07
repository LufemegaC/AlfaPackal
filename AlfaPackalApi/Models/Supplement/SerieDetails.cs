using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Models.IModels;

namespace Api_PACsServer.Models.Supplement
{
    public class SerieDetails : IAuditable
    /* Last Modified: 12/16/24 LFMG
    Add constructors */
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
