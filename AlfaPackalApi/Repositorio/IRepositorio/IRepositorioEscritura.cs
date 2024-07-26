namespace Api_PACsServer.Repositorio.IRepositorio
{
    public interface IRepositorioEscritura<T> where T : class
    {
        Task Crear(T entidad);
        //Task Actualizar(T entidad);
        //Task Remover(T entidad);
        Task Grabar();
    }
}
