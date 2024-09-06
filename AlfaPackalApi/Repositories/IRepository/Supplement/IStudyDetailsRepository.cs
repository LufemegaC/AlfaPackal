using Api_PACsServer.Modelos.Load;
using Api_PACsServer.Repository.IRepository.RepositoryBase;

namespace Api_PACsServer.Repositorio.IRepositorio.Cargas
{
    /// <summary>
    /// Represents a repository interface for handling StudyLoad entities.
    /// </summary>
    /// <author>Luis F. Méndez G.</author>
    public interface IStudyDetailsRepository : IWriteRepository<StudyDetails>, IReadRepository<StudyDetails>
    {
        /// <summary>
        /// Updates the specified StudyLoad entity.
        /// </summary>
        /// <param name="studyLoad">The StudyLoad entity to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated StudyLoad entity.</returns>
        Task<StudyDetails> Update(StudyDetails studyLoad);
    }
}
