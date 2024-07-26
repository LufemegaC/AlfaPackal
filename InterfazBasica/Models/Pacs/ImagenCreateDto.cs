using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace InterfazBasica.Models.Pacs
{
    public class ImagenCreateDto
    {
        public string? ImageComments { get; set; } //Descripción de la imagen
                                                   // Union con entidad Serie
        [JsonProperty("PACS_SerieID")]
        public int PACS_SerieID { get; set; }
        [Required, MaxLength(64)]
        public string SeriesInstanceUID { get; set; } // Identificador  
        [Required, MaxLength(64)]
        public string StudyInstanceUID { get; set; } // Identificador  
        // UID
        [Required, MaxLength(64)]
        public string SOPInstanceUID { get; set; }
        // Numero de la imagen dentro de la serie  
        [Required]
        public int ImageNumber { get; set; }
        // Direccion URL de la imagen
        public string? ImageLocation { get; set; } //Ubicación de la imagen (por ejemplo, la URL de la imagen en Azure)
        // Transfer Syntax UID para especificar la codificación de la imagen
        //[Required, MaxLength(64)]
        //public string TransferSyntaxUID { get; set; }

        // Photometric Interpretation para la interpretación de color de la imagen
        [MaxLength(16)]
        public string PhotometricInterpretation { get; set; }

        // Dimensiones de la imagen
        public int Rows { get; set; }
        public int Columns { get; set; }

        // Pixel Spacing para el espaciamiento físico de los píxeles en la imagen
        public string? PixelSpacing { get; set; }
        public int? NumberOfFrames { get; set; }

        // 09/07/24 Luis Felipe MG: Agrego las propiedades 
        // del proceso de carga
        public decimal? TotalFileSizeMB { get; set; }
        // 12/07/24 Luis Felipe MG : Agrego bandera de presencia
        public bool ExistStudy { get; set; }
    }
}
