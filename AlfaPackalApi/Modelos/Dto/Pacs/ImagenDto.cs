using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class ImagenDto
    {
        // Union con la entidad principal
        public int PACS_ImagenID { get; set; }

        // Numero de la imagen dentro de la serie
        [Display(Name = "Comentario de imagen")]
        public string ImageComments { get; set; } //Descripción de la imagen
		[Required,Display(Name = "ID de imagen"), MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        
        
        [Required, MaxLength(64),Display(Name = "Numero de imagen")]
        public int ImageNumber { get; set; }

        // 09/07/24 Luis Felipe MG: Agrego las propiedades 
        // del proceso de carga 

        // Tamaño total del estudio en MB
        public decimal? TotalFileSizeMB { get; set; }
        // Numero de Frames, Metadatos
        public int? NumberOfFrames { get; set; }

    }
}
