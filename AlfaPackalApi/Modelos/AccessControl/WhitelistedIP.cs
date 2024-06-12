using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Api_PACsServer.Modelos.Geografia;

namespace Api_PACsServer.Modelos.AccessControl
{
    public class WhitelistedIP
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Union con Servidor
        [Required]
        public int DicomServerId { get; set; }

        [ForeignKey("DicomServerId")]
        public virtual DicomServer Institution { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        public DateTime? DateRemoved { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
