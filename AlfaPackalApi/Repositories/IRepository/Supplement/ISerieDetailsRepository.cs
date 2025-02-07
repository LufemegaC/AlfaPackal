using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Repositories.IRepository.DataAccess;

namespace Api_PACsServer.Repositories.IRepository.Supplement
{
    /// <summary>
    /// Represents a repository interface for handling SerieLoad entities.
    /// </summary>
    /// <author>Luis F. Méndez G.</author> 
    public interface ISerieDetailsRepository : IWriteRepository<SerieDetails>, IReadRepository<SerieDetails>
    {
        /// <summary>
        /// Updates the specified SerieLoad entity.
        /// </summary>
        /// <param name="serieLoad">The SerieLoad entity to update.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated SerieLoad entity.</returns>
        Task<SerieDetails> Update(SerieDetails serieLoad);


        Task<List<SerieDetails>> GetDetailsByUIDs(List<string> serieInstanceUIDs);

    }
}
