using System.Net;

namespace DicomProcessingService.Dtos
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }
        public bool IsExitoso { get; set; } = true;
        public List<string> ErrorsMessages { get; set; }
        public object Resultado { get; set; }

    }
}
