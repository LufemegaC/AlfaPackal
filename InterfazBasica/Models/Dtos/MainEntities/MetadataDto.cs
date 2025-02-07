using System.ComponentModel.DataAnnotations;


namespace InterfazBasica_DCStore.Models.Dtos.MainEntities
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
        //public string? FileLocation { get; set; } // e.g., the URL of the image in Azure        
        //// Photometric Interpretation for image color interpretation
        [MaxLength(16)]
        public string PhotometricInterpretation { get; set; }
        // Image dimensions
        public int Rows { get; set; }
        public int Columns { get; set; }
        // Pixel Spacing for the physical spacing of pixels in the image
        public string? PixelSpacing { get; set; }
        // Number of frames in the image
        public int? NumberOfFrames { get; set; }
        // Image position and orientation patient
        public string? ImagePositionPatient { get; set; }
        public string? ImageOrientationPatient { get; set; }
        //Numbre of Bits for each pixel
        public int BitsAllocated { get; set; }
        // Nominal thickness and Relative position of a slice in millimeters.
        public float SliceThickness { get; set; }
        public float SliceLocation { get; set; }
        // Date and Time when the DICOM instance was created.
        public string? InstanceCreationDate { get; set; }
        public string? InstanceCreationTime { get; set; }
        // Date and Time when the content of the DICOM instance was generated.
        public string? ContentDate { get; set; }
        public string? ContentTime { get; set; }
        // -- Serie Information -- //
        // Description
        public string SeriesDescription { get; set; }
        public int? SeriesNumber { get; set; }
        // Study modality DICOM/Metadata
        public string? Modality { get; set; }
        // Start date and time of the Acquisition // 
        public DateTime? AcquisitionDateTime { get; set; }
        // Patient Position.
        [MaxLength(16)]
        public string? PatientPosition { get; set; }
        // Protocol Name
        public string? ProtocolName { get; set; }
        //// Series Date
        public string? SeriesDate { get; set; }
        // Series Time
        public string? SeriesTime { get; set; }
        // -- Study Information -- //
        // Study comments
        //public string? StudyComments { get; set; }
        // Study description DICOM
        public string? StudyDescription { get; set; }
        // Date the study was performed DICOM/Metadata
        [Required]
        public DateTime StudyDate { get; set; }
        public TimeSpan? StudyTime { get; set; }
        // Accession number DICOM/Metadata
        [MaxLength(64)]
        public string? AccessionNumber { get; set; }
        // Name of the institution performing the study DICOM/Metadata
        public string? InstitutionName { get; set; }
        // Body Part Examined
        public string? BodyPartExamined { get; set; }
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
        //-- Transaction --//
        // Transaction UID
        public string? TransactionUID { get; set; }
        // Transaction Status
        public string? TransactionStatus { get; set; }
        // Transaction Status Comment
        public string? TransactionStatusComment { get; set; }
    }
}
