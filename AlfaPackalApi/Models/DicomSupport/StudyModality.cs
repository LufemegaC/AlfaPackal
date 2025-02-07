using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Models.IModels;

namespace Api_PACsServer.Models.DicomSupport
{
    public class StudyModality : ICreate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModalityID { get; set; }
        // Relación con la entidad Study
        [ForeignKey("Study")]
        public string StudyInstanceUID { get; set; }
        public virtual Study Study { get; set; }
        // Modalidad específica (CR, CT, MR, etc.)
        [Required, StringLength(10)]
        public string Modality { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
