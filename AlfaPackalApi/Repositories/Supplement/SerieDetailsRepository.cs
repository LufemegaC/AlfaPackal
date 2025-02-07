using Api_PACsServer.Datos;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Repositories.DataAccess;
using Api_PACsServer.Repositories.IRepository.Supplement;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositories.Supplement
{
    public class SerieDetailsRepository : ReadWriteRepository<SerieDetails>, ISerieDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public SerieDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<SerieDetails>> ExecuteSerieDetailsQuery(List<string> serieInstanceUIDs)
        {
            return await _db.SerieDetails
               .Where(d => serieInstanceUIDs.Contains(d.SeriesInstanceUID))
               .ToListAsync();
        }

        public async Task<List<SerieDetails>> GetDetailsByUIDs(List<string> serieInstanceUIDs)
        {
            return await _db.SerieDetails
                .Where(d => serieInstanceUIDs.Contains(d.SeriesInstanceUID))
                .ToListAsync();
        }

        public async Task<SerieDetails> Update(SerieDetails serieLoad)
        {
            serieLoad.UpdateDate = DateTime.Now;
            _db.SerieDetails.Update(serieLoad);
            await _db.SaveChangesAsync();
            return serieLoad;
        }
    }
}
