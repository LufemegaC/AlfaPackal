using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Modelos.AccessControl
{
    public class DicomServer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(39)] // Máximo tamaño para IPv6
        public string IP { get; set; }

        [Required, StringLength(16)] // DICOM AETitles pueden tener hasta 16 caracteres
        public string AETitle { get; set; }

        public int? PuertoRedLocal { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }
        //Union con institution
        public int InstitutionId { get; set; }

        [ForeignKey("InstitutionId")]
        public virtual Institution Institution { get; set; }

    }
}
