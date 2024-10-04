using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api_PACsServer.Models
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }   // Indica si la operación fue exitosa.
        public string ErrorMessage { get; set; }  // Mensaje de error en caso de fallo, vacío si es exitoso.
        public object? ResponseData { get; set; }
        public static OperationResult Success(object data = null)
        {
            return new OperationResult { IsSuccess = true ,ResponseData = data};
        }

        public static OperationResult Failure(string errorMessage, object data = null)
        {
            return new OperationResult { IsSuccess = false, ErrorMessage = errorMessage, ResponseData = data };
        }

        public static OperationResult Warning(string warningMessage, object data = null)
        {
            return new OperationResult { IsSuccess = true, ErrorMessage = warningMessage, ResponseData = data };
        }
    }
}
