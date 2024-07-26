namespace InterfazDeUsuario.Services.IServices
{
    public interface IDataService
    {
        Task<T> GetMainList<T>(string token, int institutionId);

        Task<T> GetMainListPaginado<T>(string token, int institutionId, int pageNumber = 1, int pageSize = 4);

        
    }
}
