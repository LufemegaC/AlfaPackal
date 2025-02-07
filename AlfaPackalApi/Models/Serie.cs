using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Models.IModels;

namespace Api_PACsServer.Models
{
    public class Serie : ICreate
    // Creation: 07/11/23
    // Serie main entity
    {
        [Key, Required]
        public string SeriesInstanceUID { get; set; } // PK: SeriesInstanceUID
        [Required]
        public string StudyInstanceUID { get; set; } // FK hacia Study basada en StudyInstanceUID
        [ForeignKey("StudyInstanceUID")]
        public virtual Study Study { get; set; } // Relación con Study
        // Relación uno a uno con InstanceDetails
        public virtual SerieDetails SerieDetails { get; set; } // Relación uno a uno con SerieDetails
        // Series number
        public int SeriesNumber { get; set; }
        //Description
        public string? SeriesDescription { get; set; }
        // Study modality DICOM/Metadata
        public string Modality { get; set; }
        // Start date and time of the series
        public DateTime? SeriesDateTime { get; set; }
        // Body Part Examined
        public string? BodyPartExamined { get; set; }
        // Patient Position 
        public string? PatientPosition { get; set; }
		public string? ProtocolName { get; set; }
        public string? SeriesDate { get; set; }
        public string? SeriesTime { get; set; }
        // Instance collection
        public virtual ICollection<Instance> Instances { get; set; }
        // Control fields
        public DateTime CreationDate { get; set; }

    }
}
