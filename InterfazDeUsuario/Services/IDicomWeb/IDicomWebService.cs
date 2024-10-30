using Newtonsoft.Json.Linq;

namespace InterfazDeUsuario.Services.IDicomWeb
{
    public interface IDicomWebService
    {
        Task<JArray> GetMainListPaginado(string token, int limit, int pageNumber, int pageSize);
    }
}
