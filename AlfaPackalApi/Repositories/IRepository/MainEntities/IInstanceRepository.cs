using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Repositories.IRepository.DataAccess;

namespace Api_PACsServer.Repositories.IRepository.MainEntities
{
    public interface IInstanceRepository : IWriteRepository<Instance>, IReadRepository<Instance>
    {
        Task<List<Instance>> ExecuteInstanceQuery(QuerySpecification querySpecification);
    }
}
