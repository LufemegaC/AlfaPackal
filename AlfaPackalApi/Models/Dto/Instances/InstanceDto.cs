using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Instances
{
    public class InstanceDto
    {
        // Image description
        public string ImageComments { get; set; }
        // Association with Serie entity by internal ID
        public int PACSSerieID { get; set; }
        // SOP Class UID 
        [Required, MaxLength(64)]
        public string SOPClassUID { get; set; }
        // SOP Instance UID
        [Required, MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        // Image number within the series
        [Required]
        public int ImageNumber { get; set; }
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
    }
}
