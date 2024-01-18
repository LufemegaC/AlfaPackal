using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InterfazBasica.Models.Pacs
{
    public class SerieCreateDto
    {
        [Required, MaxLength(64)]
        public string SeriesInstanceUID { get; set; }
        [Required]
        //Estudio
        public int? PACS_EstudioID { get; set; }
        // Descripcion
        public string SeriesDescription { get; set; }
        // Identificador
        // Numero de serie en el estudio
        public int SeriesNumber { get; set; }     
    }
}
