using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class ImagenDto
    {
        [Display(Name = "ID de imagen")]
		[Required]
        public string SOPInstanceUID { get; set; }
        // Numero de la imagen dentro de la serie
        [Required, MaxLength(64),Display(Name = "Numero de imagen")]
        public int ImageNumber { get; set; }
        [Display(Name = "Comentario de imagen")]
        public string ImageComments { get; set; } //Descripción de la imagen

    }
}
