using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos.Cargas;
using Api_PACsServer.Repositorio;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio.Pacs
{
    public class SerieRepositorio : RepositorioLecturaYEscritura<Serie>, ISerieRepositorio
    {
        private readonly ApplicationDbContext _db;
        public SerieRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> ExistByInstanceUID(string serieInstanceUID)
        {
            return await _db.Series.AnyAsync(e => e.SeriesInstanceUID == serieInstanceUID);
        }

        public async Task<Serie> GetByInstanceUID(string serieInstanceUID)
        {
            return await _db.Series.FirstOrDefaultAsync(e => e.SeriesInstanceUID == serieInstanceUID);
        }
        public async Task<SerieCarga> UpdateFileSize(SerieCarga entidad)
        {
            entidad.UpdateDate = DateTime.Now;
            _db.SeriesCarga.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }

    }
}
