using InterfazDeUsuario.Models.Especificaciones;
using static InterfazDeUsuario.Utility.LocalUtility;

namespace InterfazDeUsuario.Models
{
    public class APIRequest
    {
        public APIType APIType { get; set; } = APIType.GET;
        public string Url { get; set; }
        public object RequestData { get; set; }
        public string Token { get; set; }
        public PaginationParameters Parameters { get; set; }
    }
}
