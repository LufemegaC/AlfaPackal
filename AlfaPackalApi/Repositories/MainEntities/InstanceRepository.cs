using AlfaPackalApi.Datos;
using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Repositorio.IRepositorio.Pacs;
using Api_PACsServer.Repository.DataAccess;
using Microsoft.EntityFrameworkCore;


namespace Api_PACsServer.Repositorio.Pacs
{
    public class InstanceRepository : ReadWriteRepository<Instance>, IInstanceRepository
    {
        private readonly ApplicationDbContext _db;
        public InstanceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //public async Task<IEnumerable<Instance>> GetAllBySerieInstanceUID(string serieInstanceUID)
        //{
        //    return await _db.Instances
        //                    .Where(s => s.Serie.SeriesInstanceUID == serieInstanceUID)
        //                    .ToListAsync();
        //}
    }
}
