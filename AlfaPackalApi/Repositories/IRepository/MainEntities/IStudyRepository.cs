using AlfaPackalApi.Modelos;
using Api_PACsServer.Modelos;
using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Repository.IRepository.RepositoryBase;
using Api_PACsServer.Modelos.Load;

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

        /// <summary>
        /// Executes a dynamic QIDO query based on the control and study parameters provided.
        /// Constructs a dynamic SQL query to retrieve data from the Studies and StudyDetails tables.
        /// </summary>
        /// <param name="controlParams">Control parameters including pagination, ordering, and inclusion fields.</param>
        /// <param name="studyParams">Study parameters for filtering the search.</param>
        /// <returns>A tuple containing a list of Study and StudyDetails.</returns>
        (List<Study>, List<StudyDetails>) ExecuteDynamicQidoQuery(ControlQueryParametersDto controlParams, StudyQueryParametersDto studyParams);
    }
}
