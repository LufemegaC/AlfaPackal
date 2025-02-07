using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Repositories.IRepository.DataAccess;

namespace Api_PACsServer.Repositories.IRepository.Supplement
{
    public interface IInstanceDetailsRepository : IWriteRepository<InstanceDetails>, IReadRepository<InstanceDetails>
    {
        //Task<ImagenCarga> Actualizar(ImagenCarga entidad);
        //Task<InstanceLoad> GetByPacsImageId(int pacsImagenID);
        Task<List<InstanceDetails>> GetDetailsByUIDs(List<string> sopInstanceUIDs);
    }
}
