using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Repository.IRepository.RepositoryBase;

namespace Api_PACsServer.Repositorio.IRepositorio.Cargas
{
    /// <summary>
    /// Represents a repository interface for handling SerieLoad entities.
    /// </summary>
    /// <author>Luis F. Méndez G.</author> 
    public interface ISerieLoadRepository : IWriteRepository<SerieLoad>, IReadRepository<SerieLoad>
    {
        /// <summary>
        /// Updates the specified SerieLoad entity.
        /// </summary>
        /// <param name="serieLoad">The SerieLoad entity to update.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated SerieLoad entity.</returns>
        Task<SerieLoad> Update(SerieLoad serieLoad);

    }
}
