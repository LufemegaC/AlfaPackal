using Api_PACsServer.Modelos.Cargas;

namespace Api_PACsServer.Repositorio.IRepositorio.Cargas
{
    public interface ISerieCargaRepositorio : IRepositorioEscritura<SerieCarga>, IRepositorioLectura<SerieCarga>
    {
        //Task<SerieCarga> Actualizar(SerieCarga entidad);
        Task<SerieCarga> UpdateSizeForNImage(SerieCarga serieCarga, decimal fileSizeMb);

        Task<SerieCarga> GetByImagenCargaId(int imagenCargaId);
    }
}
