using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Repositorio.IRepositorio.Cargas;
using Api_PACsServer.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio.Cargas
{
    public class SerieDetailsRepository : ReadWriteRepository<SerieDetails>, ISerieDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public SerieDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<SerieDetails> Update(SerieDetails serieLoad)
        {
            serieLoad.UpdateDate = DateTime.Now;
            _db.SeriesDetails.Update(serieLoad);
            await _db.SaveChangesAsync();
            return serieLoad;
        }
    }
}
