using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Models.DicomSupport;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Models.IModels;
using FellowOakDicom.Network;

namespace Api_PACsServer.Models
{
    public class Study : ICreate
    // Creation: 07/11/23
    // Unification of the Study and Patient entities.
    // ---
    {
        // ** Study info **//
        // Unique identifier for the study entity (DICOM Tag: (0020,000D))
        [Key, Required]
        public string StudyInstanceUID { get; set; }

        // Primary key, internal identifier for the study
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudyID { get; set; }

        // Relationship with StudyDetails entity (one-to-one)
        public virtual StudyDetails StudyDetails { get; set; }

        // Description of the study (DICOM Tag: (0008,1030))
        public string? StudyDescription { get; set; }

        // Date when the study was performed (DICOM Tag: (0008,0020))
        [Required]
        public DateTime StudyDate { get; set; }

        // Time when the study was performed (DICOM Tag: (0008,0030))
        public TimeSpan? StudyTime { get; set; }

        // Name of the institution performing the study (DICOM Tag: (0008,0080))
        public string? InstitutionName { get; set; }

        // Name of the referring physician (DICOM Tag: (0008,0090))
        public string? ReferringPhysicianName { get; set; }

        // Modalities used in the study (DICOM Tag: (0008,0061))
        public virtual ICollection<StudyModality>? ModalitiesInStudy { get; set; }

        // Collection of series in the study
        public virtual ICollection<Serie> Series { get; set; }

        // ** Patient Information **//
        // Identifier for the patient (DICOM Tag: (0010,0020))
        public string? PatientID { get; set; }

        // Full name of the patient in the format LastName^MiddleName^FirstName(s)^Prefix^Suffix
        // (DICOM Tag: (0010,0010))
        [Required]
        public string? PatientName { get; set; }

        // Patient age in years, formatted as "034Y" (DICOM Tag: (0010,1010))
        [StringLength(4)]
        public string? PatientAge { get; set; }

        // Patient sex: 'M' (Male), 'F' (Female), 'O' (Other), 'U' (Unknown) (DICOM Tag: (0010,0040))
        [StringLength(1), RegularExpression(@"[MFUO]")]
        public string? PatientSex { get; set; }

        // Patient weight in kilograms with optional one decimal place (DICOM Tag: (0010,1030))
        [RegularExpression(@"\d{1,3}(\.\d{1})?")]
        public string? PatientWeight { get; set; }

        // Patient's date of birth (DICOM Tag: (0010,0030))
        public DateTime? PatientBirthDate { get; set; }

        // Issuer of the patient ID, typically representing the institution or facility (DICOM Tag: (0010,0021))
        [StringLength(64)]
        public string? IssuerOfPatientID { get; set; }

        // Date of creation for auditing purposes (Not a DICOM field)
        public DateTime CreationDate { get; set; }
    }
}
