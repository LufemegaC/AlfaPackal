using System.ComponentModel.DataAnnotations;

namespace InterfazBasica_DCStore.Models.Dtos.MainEntities
{
    public class StudyDto
    {
        // ** Study info **//
        public string StudyInstanceUID { get; set; }
        // Study comments
        public string? StudyComments { get; set; }
        // Study description DICOM
        public string? StudyDescription { get; set; }
        public DateTime StudyDate { get; set; }
        public string? AccessionNumber { get; set; }
        // Name of the institution performing the study DICOM/Metadata
        public string? InstitutionName { get; set; }
        // Study modality DICOM/Metadata
        public string? Modality { get; set; }
        // Body Part Examined
        public string? BodyPartExamined { get; set; }
        // ** Institution **//
        public int InstitutionID { get; set; }
        // ** Patient Information **//
        // Non internal control institution added
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
        // ** Load Info
        // Number of DICOM files related to the study.
        public int? NumberOfFiles { get; set; }
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }
        // Number of series in the study.
        public int? NumberOfSeries { get; set; }
    }
}
