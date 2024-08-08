using AlfaPackalApi.Modelos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Modelos.AccessControl;
using Api_PACsServer.Modelos.IModels;

namespace Api_PACsServer.Modelos
{
    public class Study : ICreate
    // Creation: 07/26/24
    // Unification of the Study and Patient entities, as well as
    // standardizing to English.
    {
        // ** Study info **//
        // Primary key, internal study ID
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACSStudyID { get; set; }
        // UID of the study entity
        [Required]
        public string StudyInstanceUID { get; set; }
        // Study comments
        public string? StudyComments { get; set; }
        // Study description DICOM
        public string? StudyDescription { get; set; }
        // Date the study was performed DICOM/Metadata
        [Required]
        public DateTime StudyDate { get; set; }
        // Accession number DICOM/Metadata
        [MaxLength(64)]
        public string? AccessionNumber { get; set; }
        // Name of the institution performing the study DICOM/Metadata
        public string? InstitutionName { get; set; }
        // Study modality DICOM/Metadata
        public string? Modality { get; set; }
        // Body Part Examined
        public string? BodyPartExamined { get; set; }
        // ** Issuing institution (Internal control)
        [Required]
        public int InstitutionID { get; set; }
        [ForeignKey("InstitutionID")]
        public virtual Institution Institution { get; set; }
        // ** Collection of Series
        public virtual ICollection<Serie> Series { get; set; }
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
        // Additional fields suggested by DICOM structure
        [StringLength(64)]
        public string? IssuerOfPatientID { get; set; } // Issuer of the PatientID
        // Auditable info
        public DateTime CreationDate { get; set; }
    }
}
