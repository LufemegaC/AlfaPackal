using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class ImagenDto
    {   
        // Numero de la imagen dentro de la serie
        [Display(Name = "Comentario de imagen")]
        public string ImageComments { get; set; } //Descripción de la imagen
		[Required,Display(Name = "ID de imagen"), MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        
        
        [Required, MaxLength(64),Display(Name = "Numero de imagen")]
        public int ImageNumber { get; set; }


    }
}
