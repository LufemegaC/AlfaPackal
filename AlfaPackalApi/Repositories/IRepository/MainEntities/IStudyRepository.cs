using Api_PACsServer.Models.Dto.Studies;
using Api_PACsServer.Models.Dto;
using Api_PACsServer.Models;
using Api_PACsServer.Repositories.IRepository.DataAccess;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Models.Specifications;
using Microsoft.Data.SqlClient;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;

namespace Api_PACsServer.Repositories.IRepository.MainEntities
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


        Task<List<Study>> ExecuteStudyQuery(QuerySpecification querySpecification);


    }
}
