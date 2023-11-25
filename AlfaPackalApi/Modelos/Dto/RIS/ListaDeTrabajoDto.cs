using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.RIS
{
    public class ListaDeTrabajoDto
    {
        [Key]
        public int ListaID { get; set; }

        public virtual ICollection<Estudio> Estudios { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [MaxLength(20)]
        public string Estado { get; set; }
    }
}
