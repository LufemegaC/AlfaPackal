using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_PACsServer.Models.Dto.Series
{
    public class SerieUpdateDto
    {
        public int PACSSerieLoadID { get; set; }
        // Numero de imagenes relacionadas
        public int? NumberOfInstances { get; set; }
        // Tamaño total del estudio en MB
        public decimal TotalFileSizeMB { get; set; }
    }
}
