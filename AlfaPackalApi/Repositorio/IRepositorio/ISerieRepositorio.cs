using AlfaPackalApi.Modelos;

namespace AlfaPackalApi.Repositorio.IRepositorio
{
    public interface ISerieRepositorio : IRepositorio<Serie>
    {
        Task<Serie> Actualizar(Serie entidad);
        Task<bool> ExisteSeriesInstanceUID(string serieInstanceUID);
        Task<Serie> GetSerieByInstanceUID(string serieInstanceUID);
    }
}
