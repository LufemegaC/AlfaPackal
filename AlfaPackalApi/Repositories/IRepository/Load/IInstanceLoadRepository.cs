using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Repository.IRepository.RepositoryBase;

namespace Api_PACsServer.Repositorio.IRepositorio.Cargas
{
    public interface IInstanceLoadRepository: IWriteRepository<InstanceLoad>, IReadRepository<InstanceLoad>
    {
        //Task<ImagenCarga> Actualizar(ImagenCarga entidad);
        //Task<InstanceLoad> GetByPacsImageId(int pacsImagenID);
    }
}
