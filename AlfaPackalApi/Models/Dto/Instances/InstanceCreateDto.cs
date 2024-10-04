using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_PACsServer.Models.Dto.Instances
{
    public class InstanceCreateDto
    {
        // Primary Key: Composite key consisting of StudyID, SeriesNumber, and ImageNumber
        [Required]
        public string SOPInstanceUID { get; set; }
        [Required]
        public string SeriesInstanceUID { get; set; }
        [Required]
        public int InstanceNumber { get; set; }
        // Image description
        public string ImageComments { get; set; }
        // SOP Class UID 
        [Required, MaxLength(64)]
        public string SOPClassUID { get; set; }
        public string? TransferSyntaxUID { get; set; }
        // Photometric Interpretation for image color interpretation
        [MaxLength(16)]
        public string PhotometricInterpretation { get; set; }
        // Image dimensions
        public int Rows { get; set; }
        public int Columns { get; set; }
        // orientation
        public string? ImagePositionPatient { get; set; }
        public string? ImageOrientationPatient { get; set; }
        // Pixel Spacing for the physical spacing of pixels in the image
        public string? PixelSpacing { get; set; }
        // Number of frames in the image
        public int? NumberOfFrames { get; set; }
        //** Load Section **//
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }

    }
}
