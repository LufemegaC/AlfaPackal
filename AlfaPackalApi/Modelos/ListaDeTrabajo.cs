using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPackalApi.Modelos
{
    public class ListaDeTrabajo : IAuditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListaID { get; set; }

        public virtual ICollection<Estudio> Estudios { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [MaxLength(20)]
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
