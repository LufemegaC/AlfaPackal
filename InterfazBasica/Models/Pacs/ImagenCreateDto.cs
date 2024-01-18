using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FellowOakDicom;

namespace InterfazBasica.Models.Pacs
{
    public class ImagenCreateDto
    {
        // UID
        [Required, MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        // Numero de la imagen dentro de la serie
        public string ImageComments { get; set; } //Descripción de la imagen
        // Union con entidad Serie
        public int PACS_SerieID { get; set; } 
		[Required]
        public int ImageNumber { get; set; }
        // Direccion URL de la imagen
        public string? ImageLocation { get; set; } //Ubicación de la imagen (por ejemplo, la URL de la imagen en Azure)
    }
}
