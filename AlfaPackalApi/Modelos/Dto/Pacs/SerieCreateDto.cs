using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class SerieCreateDto
    {
        public int? EstudioID { get; set; }
        // Descripcion
        public string SeriesDescription { get; set; }
        // Identificador
        [Required, MaxLength(64)]
        public string SeriesInstanceUID { get; set; }
        // Numero de serie en el estudio
        public int SeriesNumber { get; set; }
    }
}
