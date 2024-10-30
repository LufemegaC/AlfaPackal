using Api_PACsServer.Modelos;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.DicomList
{
    public class StudyModality
    {
        public StudyModality(int studyId, string modality)
        {
            StudyID = studyId;
            Modality = modality;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModalityID { get; set; }
        // Relación con la entidad Study
        [ForeignKey("Study")]
        public int StudyID { get; set; }
        public virtual Study Study { get; set; }

        // Modalidad específica (CR, CT, MR, etc.)
        [Required, StringLength(10)]
        public string Modality { get; set; }

    }
}
