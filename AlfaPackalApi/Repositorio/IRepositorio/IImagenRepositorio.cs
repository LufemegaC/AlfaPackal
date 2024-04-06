using AlfaPackalApi.Modelos;

namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface IImagenRepositorio : IRepositorio<Imagen>
    {
        Task<Imagen> GetBySOPInstanceUID(string sopInstanceUID);
        Task<bool> ExistBySOPInstanceUID(string sopInstanceUID);

    }
}
