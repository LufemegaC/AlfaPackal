using InterfazDeUsuario.Models.Especificaciones;
using static Utileria.DS;

namespace InterfazDeUsuario.Models
{
    public class APIRequest
    {
        public APITipo APITipo { get; set; } = APITipo.GET;
        public string Url { get; set; }
        public object Datos { get; set; }
        public string Token { get; set; }
        public ParametrosPag Parametros { get; set; }
    }
}
