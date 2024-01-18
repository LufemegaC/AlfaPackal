using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos
{
    public class Imagen : IAuditable
    {
        // PK de la imagen
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PACS_ImagenID { get; set; }
        public string ImageComments { get; set; } //Descripción de la imagen
        // Union con entidad Serie
        public int PACS_SerieID { get; set; }
        [Required, ForeignKey("PACS_SerieID")]
        public virtual Serie Serie { get; set; }
        // UID
        [Required, MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        // Numero de la imagen dentro de la serie
		[Required]
        public int ImageNumber { get; set; }
        // Direccion URL de la imagen
        [Required]
        public string ImageLocation { get; set; } //Ubicación de la imagen (por ejemplo, la URL de la imagen en Azure)
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

    }
}
