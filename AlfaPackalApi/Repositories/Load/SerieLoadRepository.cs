using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio.Cargas
{
    public class SerieLoadRepository : ReadWriteRepository<SerieLoad>, ISerieLoadRepository
    {
        private readonly ApplicationDbContext _db;

        public SerieLoadRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<SerieLoad> Update(SerieLoad serieLoad)
        {
            serieLoad.UpdateDate = DateTime.Now;
            _db.SeriesLoad.Update(serieLoad);
            await _db.SaveChangesAsync();
            return serieLoad;
        }
    }
}
