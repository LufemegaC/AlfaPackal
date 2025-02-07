using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Studies
{
    public class StudyDto
    {
        // ** Study info **//
        public string StudyInstanceUID { get; set; }
        // internal ID
        public int StudyID { get; set; }
        // Study description DICOM
        public string? StudyDescription { get; set; }
        //Study date
        public DateTime StudyDate { get; set; }
        //Study Time
        public TimeSpan? StudyTime { get; set; }
        // Name of the institution performing the study DICOM/Metadata
        public string? InstitutionName { get; set; }
        // Referring Physician Name
        public string? ReferringPhysicianName { get; set; }
        // Modalities
        public string? ModalitiesInStudy { get; set; }
        public string? PatientID { get; set; }
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
        // ** Details info
        // Number of series in the study.
        public int? NumberOfStudyRelatedInstances { get; set; }
        // Number of DICOM files related to the study.
        public int? NumberOfStudyRelatedSeries { get; set; }
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }
    }
}
