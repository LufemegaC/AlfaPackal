using Api_PACsServer.Models;
using Api_PACsServer.Models.Dto.DicomWeb.Qido;
using Api_PACsServer.Models.Supplement;
using Api_PACsServer.Repositories.IRepository.DataAccess;
using Microsoft.Data.SqlClient;

namespace Api_PACsServer.Repositories.IRepository.MainEntities
{
    public interface ISerieRepository : IWriteRepository<Serie>, IReadRepository<Serie>
    {
        ///// <summary>
        ///// Executes a dynamic QIDO query based on provided control and study parameters.
        ///// Constructs an SQL query to retrieve data from the Series and SeriesDetails tables.
        ///// </summary>
        ///// <param name="selectClause">Specifies the SQL SELECT clause for the query.</param>
        ///// <param name="whereClause">Defines filtering conditions for the query.</param>
        ///// <param name="orderByClause">Indicates the sorting criteria for the results.</param>
        ///// <param name="parameters">A list of SQL parameters for the query.</param>
        ///// <returns>A tuple containing lists of Series and SeriesDetails.</returns>
        //Task<(List<Serie>, List<SerieDetails>)> ExecuteSerieQuery(string selectClause, string whereClause, string orderByClause, List<SqlParameter> parameters);


        /// <summary>
        /// Executes a dynamic QIDO query based on the control and study parameters provided.
        /// Constructs a dynamic SQL query to retrieve data from the Serie table.
        /// </summary>
        /// <param name="querySpecification">Control parameters including pagination, ordering, and inclusion fields.</param>
        /// <returns>A tuple containing a list of Serie .</returns>
        Task<List<Serie>> ExecuteSerieQuery(QuerySpecification querySpecification);

    }
}

