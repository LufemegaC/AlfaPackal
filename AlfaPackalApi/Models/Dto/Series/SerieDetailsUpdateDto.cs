
namespace Api_PACsServer.Models.Dto.Series
{
    public class SerieDetailsUpdateDto
    {
        public string SeriesInstanceUID { get; set; }
        // Numero de imagenes relacionadas
        public int? NumberOfSeriesRelatedInstances { get; set; }
        // Tamaño total del estudio en MB
        public decimal TotalFileSizeMB { get; set; }
    }
}
