using Api_PACsServer.Datos;
using Api_PACsServer.Repositories.IRepository.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositories.DataAccess
{
    public class WriteRepository<T> : IWriteRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DbSet;
        public WriteRepository(ApplicationDbContext db)
        {
            _db = db;
            DbSet = _db.Set<T>();
        }

        public async Task Create(T entity)
        {
            await DbSet.AddAsync(entity);
            await Save();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

    }
}
