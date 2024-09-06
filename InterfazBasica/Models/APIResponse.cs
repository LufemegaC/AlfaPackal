using System.Net;

namespace InterfazBasica.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public List<string>? ErrorsMessages { get; set; }
        public object? ResponseData { get; set; }
        public int TotalPages { get; set; }
        public APIResponse()
        {

        }
        // Constructor que acepta una lista de errores
        public APIResponse(List<string> errors = null, object resultado = null)
        {
            StatusCode = errors == null ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            IsSuccessful = errors == null || errors.Count == 0;
            ErrorsMessages = errors ?? new List<string>();
            ResponseData = resultado;
        }

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
