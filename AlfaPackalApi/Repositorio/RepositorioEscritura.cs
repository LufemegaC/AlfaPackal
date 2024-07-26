using AlfaPackalApi.Datos;
using Api_PACsServer.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio
{
    public class RepositorioEscritura<T> : IRepositorioEscritura<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DbSet;
        public RepositorioEscritura(ApplicationDbContext db)
        {
            _db = db;
            this.DbSet = _db.Set<T>();
        }

        public async Task Crear(T entidad)
        {
            await DbSet.AddAsync(entidad);
            await Grabar();
        }

        public async Task Grabar()
        {
            await _db.SaveChangesAsync();
        }


    }
}
