using AlfaPackalApi.Modelos;

namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface ISerieRepositorio : IRepositorio<Serie>
    {
        //Task<Serie> Actualizar(Serie entidad);
        Task<Serie> GetByInstanceUID(string serieInstanceUID);
        Task<bool> ExistByInstanceUID(string serieInstanceUID);
    }
}
