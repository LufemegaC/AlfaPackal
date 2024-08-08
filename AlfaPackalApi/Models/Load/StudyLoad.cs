using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Modelos.IModels;

namespace Api_PACsServer.Modelos.Load
{
    //Represents the load information related to a study, including size and number of related files and series.
    public class StudyLoad : IAuditable
    {
        // Auto-generated primary key.
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACSStudyLoadID { get; set; }
        // Foreign key to the main Study entity.
        public int PACSStudyID { get; set; } 
        [Required, ForeignKey("PACSStudyID")]
        public virtual Study Study { get; set; }
        // Number of DICOM files related to the study.
        public int? NumberOfFiles { get; set; }
        // Total size of the study in MB.
        public decimal TotalFileSizeMB { get; set; }
        // Number of series in the study.
        public int? NumberOfSeries { get; set; }
        // Date when the record was created.
        public DateTime CreationDate { get; set; }
        // Date when the record was last updated.
        public DateTime UpdateDate { get; set; }

    }
}
