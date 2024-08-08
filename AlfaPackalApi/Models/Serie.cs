using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.IModels;

namespace AlfaPackalApi.Modelos
{
    public class Serie : ICreate
    {
        // Auto-incremented internal ID generated automatically
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACSSerieID { get; set; } // Internal ID
        // Internal ID of the parent Study, link to Study
        [Required]
        public int? PACSStudyID { get; set; }
        [ForeignKey("PACSStudyID")]
        public virtual Study Study { get; set; }
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
        // Instance collection
        public virtual ICollection<Instance> Instances { get; set; }
        // Control fields
        public DateTime CreationDate { get; set; }
    }
}
