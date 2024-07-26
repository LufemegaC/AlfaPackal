using AlfaPackalApi.Datos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace Api_PACsServer.Repositorio
{
    public class RepositorioLecturaYEscritura<T> : IRepositorioEscritura<T>, IRepositorioLectura<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DbSet;
        public RepositorioLecturaYEscritura(ApplicationDbContext db)
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

        public async Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true, string incluirPropiedades = null)
        {
            IQueryable<T> query = DbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }

            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null, string incluirPropiedades = null)
        {
            IQueryable<T> query = DbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.ToListAsync();
        }

        public PagedList<T> ObtenerTodosPaginado(ParametrosPag parametros, Expression<Func<T, bool>>? filtro = null, string incluirPropiedades = null)
        {
            IQueryable<T> query = DbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }

            }

            return PagedList<T>.ToPagedList(query, parametros.PageNumber, parametros.PageSize);
        }

    }
}
