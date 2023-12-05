using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace AlfaPackalApi.Repositorio
{
    public class SerieRepositorio: Repositorio<Serie>, ISerieRepositorio
    {
        private readonly ApplicationDbContext _db;
        public SerieRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Serie> Actualizar(Serie entidad)
        {
            //entidad.FechaActualizacion = DateTime.Now; 
            _db.Series.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }

        public async Task<bool> ExisteSeriesInstanceUID(string serieInstanceUID)
        {
            return await _db.Series.AnyAsync(e => e.SeriesInstanceUID == serieInstanceUID);
        }

        public async Task<Serie> GetSerieByInstanceUID(string serieInstanceUID)
        {
            return await _db.Series.FirstOrDefaultAsync(e => e.SeriesInstanceUID == serieInstanceUID);
        }

    }
}
