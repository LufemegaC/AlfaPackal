using Api_PACsServer.Modelos.Cargas;

namespace Api_PACsServer.Repositorio.IRepositorio.Cargas
{
    public interface IEstudioCargaRepositorio : IRepositorioEscritura<EstudioCarga>, IRepositorioLectura<EstudioCarga>
    {
        Task<EstudioCarga> GetByImageCargaId(int imagenCargaId);

        Task<EstudioCarga> GetBySerieCargaId(int serieCargaId);


        //Task<EstudioCarga> Actualizar(EstudioCarga entidad);

        Task<EstudioCarga> UpdateSizeForNImage(EstudioCarga estudioCarga, decimal FileSizeMb);

        Task<EstudioCarga> UpdateSizeForForNSerie(EstudioCarga estudioCarga);


    }
}
