using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Models.IModels;

namespace Api_PACsServer.Models
{
    public class Instance : ICreate
    // Creation: 07/11/23
    // Image/instance main entity
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
        public string? TransferSyntaxUID { get; set; }
        // Image description
        public string? ImageComments { get; set; }

        // Photometric Interpretation for image color interpretation
        [MaxLength(16)]
        public string? PhotometricInterpretation { get; set; }
        // Image dimensions
        public int Rows { get; set; }
        public int Columns { get; set; }
        // orientation
        public string? ImagePositionPatient { get; set; }
        public string? ImageOrientationPatient { get; set; }
        // Pixel Spacing for the physical spacing of pixels in the image
        public string? PixelSpacing { get; set; }
        // Número de bits asignados por píxel en los datos de la imagen.
        public int BitsAllocated { get; set; }
        //Espesor nominal de una rebanada en milímetros.
        public float SliceThickness { get; set; }
        // Posición relativa de la rebanada dentro del sistema de coordenadas del paciente (en milímetros).
        public float SliceLocation { get; set; }
        public string? InstanceCreationDate { get; set; }
        public string? InstanceCreationTime { get; set; }
        public string? ContentDate { get; set; }
        public string? ContentTime { get; set; }
        // Number of frames in the image
        public int? NumberOfFrames { get; set; }
        //-- Transaction --//
        // Transaction UID
        [Required]
        public string TransactionUID { get; set; }

        // Control fields
        public DateTime CreationDate { get; set; }
    }
}
