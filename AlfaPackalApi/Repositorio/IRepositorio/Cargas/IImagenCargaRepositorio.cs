using Api_PACsServer.Modelos.Cargas;

namespace Api_PACsServer.Repositorio.IRepositorio.Cargas
{
    public interface IImagenCargaRepositorio: IRepositorioEscritura<ImagenCarga>, IRepositorioLectura<ImagenCarga>
    {
        //Task<ImagenCarga> Actualizar(ImagenCarga entidad);
        Task<ImagenCarga> GetByPacsImageId(int pacsImagenID);
    }
}
