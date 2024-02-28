using static Utileria.DS;

namespace DicomProcessingService.Dtos
{
    public class APIRequest
    {
        public APITipo APITipo { get; set; } = APITipo.GET;
        public string Url { get; set; }
        public object Datos { get; set; }
    }
}
