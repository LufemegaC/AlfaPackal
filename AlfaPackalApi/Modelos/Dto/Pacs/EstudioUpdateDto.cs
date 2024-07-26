using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Utileria.DicomValues;

namespace AlfaPackalApi.Modelos.Dto.Pacs
{
    public class EstudioUpdateDto
    {
        // ID Interno de estudio
        public int PACS_EstudioID { get; set; }
        //Llave primaria
        public int? NumberOfFiles { get; set; }
        // Tamaño total del estudio en MB
        public decimal? TotalFileSizeMB { get; set; }
        // Número de series
        public int? NumberOfSeries { get; set; }
    }
}
