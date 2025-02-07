using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Models.IModels;

namespace Api_PACsServer.Models.Supplement
{
    public class InstanceDetails : IAuditable
    /* Last Modified: 12/16/24 LFMG
    Add constructors */
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
