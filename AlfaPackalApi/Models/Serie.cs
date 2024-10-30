using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.IModels;
using Api_PACsServer.Modelos.Load;

namespace AlfaPackalApi.Modelos
{
    public class Serie : ICreate
    {
        [Key, Required]
        public string SeriesInstanceUID { get; set; } // PK: SeriesInstanceUID
        [Required]
        public string StudyInstanceUID { get; set; } // FK hacia Study basada en StudyInstanceUID
        [ForeignKey("StudyInstanceUID")]
        public virtual Study Study { get; set; } // Relación con Study
        // Relación uno a uno con InstanceDetails
        public virtual SerieDetails SerieDetails { get; set; } // Relación uno a uno con InstanceDetails
        // Series number
        public int SeriesNumber { get; set; }
        //Description
        public string? SeriesDescription { get; set; }
        // Study modality DICOM/Metadata
        public string? Modality { get; set; }
        // Start date and time of the series
        public DateTime? SeriesDateTime { get; set; }
        // Body Part Examined
        public string? BodyPartExamined { get; set; }
        // Patient Position 
        public string? PatientPosition { get; set; }     
        // Instance collection
        public virtual ICollection<Instance> Instances { get; set; }
        // Control fields
        public DateTime CreationDate { get; set; }
        
    }
}
