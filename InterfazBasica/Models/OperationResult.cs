
namespace InterfazBasica_DCStore.Models
{
    public class OperationResult
    {
        public bool Success { get; private set; }
        public List<string> ErrorsMessages { get; set; } = new List<string>();
        public OperationResult(bool success)
        {
            Success = success;
        }
        // Metodo para incializar en procesos de validacion
        public static OperationResult CreateForValidation()
        {
            return new OperationResult(true);
        }

        // Método para agregar errores a la lista
        public void AddError(string message, string? data = null)
        {
            if(data != null)
            {
                ErrorsMessages.Add(message + ":" + data);
            }
            else
            {
                ErrorsMessages.Add(message);
            }
            Success = false;
        }


    }
}
