using FellowOakDicom.Network;
using System.Net;

namespace InterfazBasica.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsExitoso { get; set; } = true;
        public List<string>? ErrorsMessages { get; set; }
        public object? Resultado { get; set; }
        public int PacsResourceId { get; set; } = 0;
        public string? GeneratedServId { get; set; }
        public string? ResultadoJson { get; set; }

        public APIResponse()
        {

        }
        // Constructor que acepta una lista de errores
        public APIResponse(List<string> errors = null,object resultado = null)
        {
            StatusCode = errors == null ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            IsExitoso = errors == null || errors.Count == 0;
            ErrorsMessages = errors ?? new List<string>();
            Resultado = resultado;
        }

        // Si aún deseas tener un campo de solo lectura predefinido similar a NoValidEntity, podrías hacerlo de la siguiente manera
        public static APIResponse NoValidEntity(List<string> errors, object resultado)
        {
            return new APIResponse(errors, resultado);
        }

        public static APIResponse SuccessCase(object resultado)
        {
            return new APIResponse(null, resultado);
        }

        public static APIResponse Initial()
        {
            return new APIResponse();
        }
    }
}
