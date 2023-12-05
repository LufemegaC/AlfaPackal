using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos
{
    public class Serie : IAuditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SerieID { get; set; }
        //Estudio
        public int? EstudioID { get; set; }
        [ForeignKey("EstudioID")]
        public virtual Estudio Estudio { get; set; }
        // Descripcion
        public string SeriesDescription { get; set; }
        // Identificador
        [Required, MaxLength(64)]
        public string SeriesInstanceUID { get; set; }
        // Numero de serie en el estudio
        public int SeriesNumber { get; set; }
        // Imagenes de la serie
        public virtual ICollection<Imagen> Imagenes { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
