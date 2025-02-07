using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Models;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Repositories.IRepository.DataAccess;

namespace Api_PACsServer.Repositories.IRepository.Supplement
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

        Task<List<StudyDetails>> GetDetailsByUIDs(List<string> studyInstanceUIDs);
    }
}
