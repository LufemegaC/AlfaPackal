using AlfaPackalApi.Modelos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Modelos.IModels;

namespace Api_PACsServer.Modelos.Load
{
    public class SerieLoad : IAuditable
    {
        // Auto-generated primary key
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACSSerieLoadID { get; set; }
        // Foreign key to the main entity
        public int PACSSerieID { get; set; }
        [Required, ForeignKey("PACSSerieID")]
        public virtual Serie Serie { get; set; }
        // Number of related instances
        public int? NumberOfInstances { get; set; }
        // Total size of the series in MB
        public decimal TotalFileSizeMB { get; set; }
        // Audit fields
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
