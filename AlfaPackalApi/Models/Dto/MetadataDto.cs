﻿using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto
{
    public class MetadataDto
    {
        // SOP Class UID 
        [Required, MaxLength(64)]
        public string SOPClassUID { get; set; }
        // SOP Instance UID
        [Required, MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        // Instance UID  
        [Required, MaxLength(64)]
        public string SeriesInstanceUID { get; set; }
        // UID of the study entity
        [Required, MaxLength(64)]
        public string StudyInstanceUID { get; set; }
        // Transfer Syntax UID
        [MaxLength(64)]
        public string? TransferSyntaxUID { get; set; }

        // -- Instance information -- //
        // Image description
        public string ImageComments { get; set; }
        // image number within the series
        [Required]
        public int InstanceNumber { get; set; }
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
        public string? ImagePositionPatient { get; set; }
        public string? ImageOrientationPatient { get; set; }
        // -- Serie Information -- //
        // Description
        public string SeriesDescription { get; set; }
        public int? SeriesNumber { get; set; }
        // Study modality DICOM/Metadata
        public string? Modality { get; set; }
        // Start date and time of the series
        public DateTime? SeriesDateTime { get; set; }

        // Patient Position.
        [MaxLength(16)]
        public string? PatientPosition { get; set; }
        // -- Study Information -- //
        // Study comments
        public string? StudyComments { get; set; }
        // Study description DICOM
        public string? StudyDescription { get; set; }
        // Date the study was performed DICOM/Metadata
        [Required]
        public DateTime StudyDate { get; set; }
        public TimeSpan StudyTime { get; set; }
        // Accession number DICOM/Metadata
        [MaxLength(64)]
        public string? AccessionNumber { get; set; }
        // Name of the institution performing the study DICOM/Metadata
        public string? InstitutionName { get; set; }
        // Body Part Examined
        public string? BodyPartExamined { get; set; }
        // Issuing institution (Internal control)
        [Required]
        public int InstitutionID { get; set; }
        // ** Patient Information **//
        // Full patient name 
        // LastName^MiddleName^FirstName(s)^Prefix^Suffix
        [Required]
        public string? PatientName { get; set; }
        // Patient age
        [StringLength(4)] // DICOM format for age, e.g. "034Y"
        public string? PatientAge { get; set; }
        // Patient sex
        [StringLength(1), RegularExpression(@"[MFUO]")] // Includes 'U' for unknown and 'O' for other
        public string? PatientSex { get; set; }
        // Patient weight
        [RegularExpression(@"\d{1,3}(\.\d{1})?")] // Format for weight in kg with one decimal
        public string? PatientWeight { get; set; }
        public DateTime? PatientBirthDate { get; set; }
        // Additional fields suggested by DICOM structure
        [StringLength(64)]
        public string? IssuerOfPatientID { get; set; } // Issuer of the PatientID
        //** Load Section **//
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }

    }
}