using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.IModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Modelos.Load
{
    public class InstanceDetails : IAuditable
    {

        // SOP Instance UID
        [Key, Required]
        public string SOPInstanceUID { get; set; }
        // Foreign Key 
        [Required, ForeignKey("SOPInstanceUID")]
        public virtual Instance Instance { get; set; }
        // URL of the Instance location
        public string? FileLocation { get; set; } // e.g., the URL of the image in Azure
        // Total size of the instance in MB
        public decimal TotalFileSizeMB { get; set; }
        // Audit fields
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
