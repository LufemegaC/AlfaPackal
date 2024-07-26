using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.Cargas;
using Api_PACsServer.Repositorio.IRepositorio;

namespace Api_PACsServer.Repositorio.IRepositorio.Pacs
{
    public interface ISerieRepositorio : IRepositorioEscritura<Serie>, IRepositorioLectura<Serie>
    {
        Task<Serie> GetByInstanceUID(string serieInstanceUID);
        Task<bool> ExistByInstanceUID(string serieInstanceUID);
    }
}
