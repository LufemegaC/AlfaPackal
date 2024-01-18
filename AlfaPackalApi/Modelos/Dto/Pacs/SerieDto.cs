using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class SerieDto
    {
        [Required, MaxLength(64), Display(Name = "Id de Instancia")]
        public string SeriesInstanceUID { get; set; }
        // Numero de serie en el estudio
        [Display(Name = "Id del Paciente")]
        // Descripcion
        public string? SeriesDescription { get; set; }
        [Display(Name = "Numero de Serie")]
        // Identificador de serie
        public int? SeriesNumber { get; set; }
    }
}
