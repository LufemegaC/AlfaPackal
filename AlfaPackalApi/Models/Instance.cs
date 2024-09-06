using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.IModels;
using Api_PACsServer.Modelos.Load;

namespace Api_PACsServer.Modelos
{
    public class Instance : ICreate
    {

        // SOP Instance UID
        [Key, Required]
        public string SOPInstanceUID { get; set; }
        [Required]
        public string SeriesInstanceUID { get; set; } // FK hacia Serie basada en SeriesInstanceUID
        [ForeignKey("SeriesInstanceUID")]
        public virtual Serie Serie { get; set; } // Relación con Serie
        public virtual InstanceDetails InstanceDetails { get; set; }
        public int InstanceNumber { get; set; }  // Part of the composite PK, unique within the series
         // SOP Class UID 
        [Required, MaxLength(64)]
        public string SOPClassUID { get; set; }
        // Image description
        public string ImageComments { get; set; }
        // URL of the Instance location
        public string? FileLocation { get; set; } // e.g., the URL of the image in Azure
        // Photometric Interpretation for image color interpretation
        [MaxLength(16)]
        public string PhotometricInterpretation { get; set; }
        // Image dimensions
        public int Rows { get; set; }
        public int Columns { get; set; }

        // Pixel Spacing for the physical spacing of pixels in the image
        public string? PixelSpacing { get; set; }

        // Number of frames in the image
        public int? NumberOfFrames { get; set; }

        // Control fields
        public DateTime CreationDate { get; set; }
    }
}
