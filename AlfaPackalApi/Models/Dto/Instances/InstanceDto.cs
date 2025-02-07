using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_PACsServer.Models.Dto.Instances
{
    public class InstanceDto
    {
        [Required]
        public string SOPInstanceUID { get; set; }
        // Photometric Interpretation for image color interpretation
        public int InstanceNumber { get; set; }

        // SOP Class UID 
        [MaxLength(64)]
        public string SOPClassUID { get; set; }
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
        public string TransactionUID { get; set; }
    }
}
