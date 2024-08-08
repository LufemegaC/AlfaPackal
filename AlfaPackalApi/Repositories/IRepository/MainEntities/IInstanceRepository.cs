using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Repository.IRepository.RepositoryBase;

namespace Api_PACsServer.Repositorio.IRepositorio.Pacs
{
    public interface IInstanceRepository : IWriteRepository<Instance>, IReadRepository<Instance>
    {
        
    }
}
