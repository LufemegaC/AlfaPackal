using Api_PACsServer.Datos;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Repositories.DataAccess;
using Api_PACsServer.Repositories.IRepository.Supplement;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositories.Supplement
{
    public class InstanceDetailsRepository : ReadWriteRepository<InstanceDetails>, IInstanceDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public InstanceDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<InstanceDetails>> GetDetailsByUIDs(List<string> sopInstanceUIDs)
        {
            return await _db.InstanceDetails
               .Where(d => sopInstanceUIDs.Contains(d.SOPInstanceUID))
               .ToListAsync();

        }


    }
}
