using AlfaPackalApi.Modelos;

namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface IImagenRepositorio : IRepositorio<Imagen>
    {
        Task<Imagen> Actualizar(Imagen entidad);
        Task<bool> ExisteImagenInstanceUID(string imagenInstanceUID);
        Task<Imagen> GetImageByInstanceUID(string imagenInstanceUID);

    }
}
