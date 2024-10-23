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
        // Image description
        public string? ImageComments { get; set; }
        // SOP Class UID 
        [MaxLength(64)]
        public string SOPClassUID { get; set; }
        // PhotometricInterpretation
        [MaxLength(16)]
        public string PhotometricInterpretation { get; set; }
        // Image dimensions
        public int Rows { get; set; }
        public int Columns { get; set; }
        // Pixel Spacing for the physical spacing of pixels in the image
        public string? PixelSpacing { get; set; }
        // Number of frames in the image
        public int? NumberOfFrames { get; set; }
        public string TransactionUID { get; set; }
    }
}
