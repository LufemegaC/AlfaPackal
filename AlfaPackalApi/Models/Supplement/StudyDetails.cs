using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Modelos.IModels;
using Api_PACsServer.Models.DicomList;

namespace Api_PACsServer.Modelos.Load
{
    //Represents the load information related to a study, including size and number of related files and series.
    public class StudyDetails : IAuditable
    {
        // StudyId - N
        // UID of the study entity
        [Key, Required]
        public string StudyInstanceUID { get; set; }
        // Foreign key to the main Study entity.
        // ** Foreign Key: Relación con la tabla Study mediante StudyInstanceUID**
        [Required, ForeignKey("StudyInstanceUID")]
        public virtual Study Study { get; set; } // Relación con la tabla Study usando StudyInstanceUID

        // Number of series in the study.
        public int? NumberOfStudyRelatedInstances { get; set; }
        // Number of DICOM files related to the study.
        public int? NumberOfStudyRelatedSeries { get; set; }
        // Accession number DICOM/Metadata
        [MaxLength(64)]
        public string? AccessionNumber { get; set; }
        
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }
        // Date when the record was created.
        public DateTime CreationDate { get; set; }
        // Date when the record was last updated.
        public DateTime UpdateDate { get; set; }
    }
}
