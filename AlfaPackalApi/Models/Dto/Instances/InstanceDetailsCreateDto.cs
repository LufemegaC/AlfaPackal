using Api_PACsServer.Modelos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Instances
{
    public class InstanceDetailsCreateDto
    {
        public InstanceDetailsCreateDto(string instanceUID, decimal sizeFile)
        {
            SOPInstanceUID = instanceUID;
            TotalFileSizeMB = sizeFile;
            CreationDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        // Auto-generated primary key
        [Required]
        public string SOPInstanceUID { get; set; }
        // Total size of the instance in MB
        public decimal TotalFileSizeMB { get; set; }
        // Audit fields
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
