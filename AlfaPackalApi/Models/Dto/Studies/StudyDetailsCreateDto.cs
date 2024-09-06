using Api_PACsServer.Modelos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Studies
{
    public class StudyDetailsCreateDto
    {
        // Constructor que inicializa todos los campos
        public StudyDetailsCreateDto(string InstanceUID, decimal totalSizeFile)
        {
            StudyInstanceUID = InstanceUID;
            TotalFileSizeMB = totalSizeFile;
            NumberOfStudyRelatedInstances = 1;
            NumberOfStudyRelatedSeries = 1;
            CreationDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }
        // StudyId - N
        [Required]
        public string StudyInstanceUID { get; set; }
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
