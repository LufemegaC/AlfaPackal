﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.IModels;

namespace Api_PACsServer.Modelos
{
    public class Instance : ICreate
    {
        // Primary key of the image
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACSInstanceID { get; set; }
        // Image description
        public string ImageComments { get; set; }
        // Association with Serie entity by internal ID
        public int PACSSerieID { get; set; }
        [Required, ForeignKey("PACSSerieID")]
        public virtual Serie Serie { get; set; }
        // SOP Class UID 
        [Required, MaxLength(64)]
        public string SOPClassUID { get; set; }
        // SOP Instance UID
        [Required, MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        // Image number within the series
        [Required]
        public int ImageNumber { get; set; }
        // URL of the Instance location
        public string? InstanceLocation { get; set; } // e.g., the URL of the image in Azure
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
