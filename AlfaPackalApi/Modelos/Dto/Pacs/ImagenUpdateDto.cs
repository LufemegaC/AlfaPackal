using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class ImagenUpdateDto
    {
        public int PACS_ImagenID { get; set; }
        // Tamaño total del estudio en MB
        public decimal? TotalFileSizeMB { get; set; }
        // Numero de Frames, Metadatos
        public int? NumberOfFrames { get; set; }
    }
}
