using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.Cargas;
using Api_PACsServer.Repositorio.IRepositorio;

namespace Api_PACsServer.Repositorio.IRepositorio.Pacs
{
    public interface IImagenRepositorio : IRepositorioEscritura<Imagen>, IRepositorioLectura<Imagen>
    {
        Task<Imagen> GetByInstanceUID(string sopInstanceUID);
        Task<bool> ExistBySOPInstanceUID(string sopInstanceUID);
    }
}
