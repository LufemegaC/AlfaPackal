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
        /// Gets the most recent studies for a specific institution with pagination options.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The size of each page to be returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains
        PagedList<Study> GetStudies(int pageNumber, int pageSize);
    }
}
