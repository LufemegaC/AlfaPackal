namespace InterfazDeUsuario.Services.IServices
{
    public interface IDataService
    {
        Task<T> GetMainList<T>(string token);
    }
}
