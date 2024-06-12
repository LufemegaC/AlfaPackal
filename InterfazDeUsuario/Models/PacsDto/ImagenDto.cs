using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace InterfazDeUsuario.Models.Pacs
{
    public class ImagenDto
    {
        [Required, Display(Name = "ID de imagen"), MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        // Numero de la imagen dentro de la serie
        [Display(Name = "Comentario de imagen")]
        public string ImageComments { get; set; } //Descripción de la imagen
        [Required, MaxLength(64), Display(Name = "Numero de imagen")]
        public int ImageNumber { get; set; }
    }
}
