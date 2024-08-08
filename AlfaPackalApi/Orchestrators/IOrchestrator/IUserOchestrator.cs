using Api_PACsServer.Modelos.Especificaciones;
using Api_PACsServer.Models.Dto;

namespace Api_PACsServer.Orchestrators.IOrchestrator
{
    public interface IUserOchestrator
    {
        /// <summary>
        /// Retrieves the most recent studies for a specified institution, with pagination.
        /// </summary>
        /// <param name="parameters">The pagination parameters, which include the institution ID, page number, etc.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable list of StudyDto objects.</returns>
        UserStudiesListDto GetRecentStudies(PaginationParameters parameters);
    }
}
