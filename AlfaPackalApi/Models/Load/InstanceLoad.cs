using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.IModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Modelos.Load
{
    public class InstanceLoad : IAuditable
    {
        // Auto-generated primary key
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACSInstanceLoadID { get; set; }
        // Foreign key to the main entity
        public int PACSInstanceID { get; set; }
        [Required, ForeignKey("PACSInstanceID")]
        public virtual Instance Instance { get; set; }
        // Total size of the instance in MB
        public decimal TotalFileSizeMB { get; set; }
        // Audit fields
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
