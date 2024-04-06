using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using AlfaPackalApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace AlfaPackalApi.Repositorio
{
    public class SerieRepositorio : Repositorio<Serie>, ISerieRepositorio
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


    }
}
