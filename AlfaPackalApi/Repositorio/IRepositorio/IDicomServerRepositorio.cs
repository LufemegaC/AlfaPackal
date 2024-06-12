namespace Api_PACsServer.Repositorio.IRepositorio
{
    public interface IDicomServerRepositorio
    {
        Task<string> GenerateDicomServerToken(string ipAddress);
        Task<string> RenewTokenIfNeeded(string currentToken, string ipAddress);
    }
}
