using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Repository.IRepository.RepositoryBase;

namespace Api_PACsServer.Repositorio.IRepositorio.Pacs
{
    /// <summary>
    /// Represents a repository interface for handling Study entities.
    /// </summary>
    /// <author>Luis F. Méndez G.</author>
    public interface IStudyRepository : IWriteRepository<Study>, IReadRepository<Study>
    {
        /// <summary>
        /// Gets the most recent studies for a specific institution.
        /// </summary>
        /// <param name="institutionId">The ID of the institution to filter studies by.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of recent Study entities.</returns>
        PagedList<Study> GetRecentStudies(PaginationParameters parameters);
    }
}
