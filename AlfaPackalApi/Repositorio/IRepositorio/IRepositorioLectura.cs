using Api_PACsServer.Modelos.Especificaciones;
using System.Linq.Expressions;

namespace Api_PACsServer.Repositorio.IRepositorio
{
    public interface IRepositorioLectura<T> where T : class
    {
        Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null, string incluirPropiedades = null);
        PagedList<T> ObtenerTodosPaginado(ParametrosPag parametros, Expression<Func<T, bool>>? filtro = null, string incluirPropiedades = null);
        Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true, string incluirPropiedades = null);
    }
}
