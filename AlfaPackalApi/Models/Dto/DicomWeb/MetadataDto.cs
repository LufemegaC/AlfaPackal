using Api_PACsServer.Models.Attributes;
using FellowOakDicom.Network;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.DicomWeb
{
    public class MetadataDto
    {
        // -- Main information -- //

        // SOP Class UID 
        [Required, MaxLength(64)]
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string SOPClassUID { get; set; }
        // SOP Instance UID
        [Required, MaxLength(64)]
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string SOPInstanceUID { get; set; }
        // Instance UID  
        [Required, MaxLength(64)]
        [DicomLevel(DicomQueryRetrieveLevel.Series)]
        public string SeriesInstanceUID { get; set; }
        // UID of the study entity
        [Required, MaxLength(64)]
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public string StudyInstanceUID { get; set; }
        // Transfer Syntax UID
        [MaxLength(64)]
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string? TransferSyntaxUID { get; set; }

        // -- Instance information -- //
        // Image description
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string ImageComments { get; set; }
        // image number within the series
        [Required]
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public int InstanceNumber { get; set; }
        //// Photometric Interpretation for image color interpretation
        [MaxLength(16)]
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string PhotometricInterpretation { get; set; }
        // Image dimensions
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public int Rows { get; set; }
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public int Columns { get; set; }
        // Pixel Spacing for the physical spacing of pixels in the image
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string? PixelSpacing { get; set; }
        // Number of frames in the image
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public int? NumberOfFrames { get; set; }
        // Image position and orientation patient
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string? ImagePositionPatient { get; set; }
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string? ImageOrientationPatient { get; set; }
        //Numbre of Bits for each pixel
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public int BitsAllocated { get; set; }
        // Nominal thickness and Relative position of a slice in millimeters.
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public float SliceThickness { get; set; }
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public float SliceLocation { get; set; }
        // Date and Time when the DICOM instance was created.
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string? InstanceCreationDate { get; set; }
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string? InstanceCreationTime { get; set; }
        // Date and Time when the content of the DICOM instance was generated.
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string? ContentDate { get; set; }
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public string? ContentTime { get; set; }
        // Start date and time of the Acquisition
        [DicomLevel(DicomQueryRetrieveLevel.Image)]
        public DateTime? AcquisitionDateTime { get; set; }
        // -- Serie Information -- //
        // Description
        [DicomLevel(DicomQueryRetrieveLevel.Series)]
        public string SeriesDescription { get; set; }
        [DicomLevel(DicomQueryRetrieveLevel.Series)]
        public int? SeriesNumber { get; set; }
        // Study modality DICOM/Metadata
        [DicomLevel(DicomQueryRetrieveLevel.Series)]
        public string? Modality { get; set; }
        // Patient Position.
        [MaxLength(16)]
        [DicomLevel(DicomQueryRetrieveLevel.Series)]
        public string? PatientPosition { get; set; }
        // Protocol Name
        [DicomLevel(DicomQueryRetrieveLevel.Series)]
        public string? ProtocolName { get; set; }
        // Series Date
        [DicomLevel(DicomQueryRetrieveLevel.Series)]
        public string? SeriesDate { get; set; }
        // Series Time
        [DicomLevel(DicomQueryRetrieveLevel.Series)]
        public string? SeriesTime { get; set; }

        // -- Study Information -- //
        // Study comments
        //[DicomLevel(DicomQueryRetrieveLevel.Study)]
        //public string? StudyComments { get; set; }
        // Study description DICOM
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public string? StudyDescription { get; set; }
        // Date the study was performed DICOM/Metadata
        [Required]
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public DateTime StudyDate { get; set; }
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public TimeSpan? StudyTime { get; set; }
        // Accession number DICOM/Metadata
        [MaxLength(64)]
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public string? AccessionNumber { get; set; }
        // Name of the institution performing the study DICOM/Metadata
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public string? InstitutionName { get; set; }
        // Body Part Examined
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public string? BodyPartExamined { get; set; }

        // ** Patient Information **//
        // Full patient name 
        // LastName^MiddleName^FirstName(s)^Prefix^Suffix
        [Required]
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public string? PatientName { get; set; }
        // Patient age
        [StringLength(4)] // DICOM format for age, e.g. "034Y"
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public string? PatientAge { get; set; }
        // Patient sex
        [StringLength(1), RegularExpression(@"[MFUO]")] // Includes 'U' for unknown and 'O' for other
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public string? PatientSex { get; set; }
        // Patient weight
        [RegularExpression(@"\d{1,3}(\.\d{1})?")] // Format for weight in kg with one decimal
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public string? PatientWeight { get; set; }
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public DateTime? PatientBirthDate { get; set; }
        // Additional fields suggested by DICOM structure
        [StringLength(64)]
        [DicomLevel(DicomQueryRetrieveLevel.Study)]
        public string? IssuerOfPatientID { get; set; } // Issuer of the PatientID

        //-- Transaction --//
        // Transaction UID
        public string? TransactionUID { get; set; }
        // Transaction Status
        public string? TransactionStatus { get; set; }
        // Transaction Status Comment
        public string? TransactionStatusComment { get; set; }

        // -- Load Section -- //
        public decimal TotalFileSizeMB { get; set; }

    }
}
