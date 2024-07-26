using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class SerieUpdateDto
    {
        public int PACS_SerieID { get; set; } //ID interno
        // Numero de imagenes relacionadas
        public int? NumberOfImages { get; set; }
        // Tamaño total del estudio en MB
        public decimal? TotalFileSizeMB { get; set; }
    }
}
