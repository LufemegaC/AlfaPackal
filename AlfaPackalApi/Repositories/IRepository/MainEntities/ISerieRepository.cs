using AlfaPackalApi.Modelos;
using Api_PACsServer.Repository.IRepository.RepositoryBase;

namespace Api_PACsServer.Repositorio.IRepositorio.Pacs
{
    public interface ISerieRepository : IWriteRepository<Serie>, IReadRepository<Serie>
    {
        
    }
}
