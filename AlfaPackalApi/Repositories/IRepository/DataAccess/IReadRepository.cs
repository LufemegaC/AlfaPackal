using Api_PACsServer.Models.Specifications;
using System.Linq.Expressions;

namespace Api_PACsServer.Repositories.IRepository.DataAccess
{
    /// <summary>
    /// Represents a repository for reading entities.
    /// </summary>
    /// <author>Luis F. Méndez G.</author>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IReadRepository<T> where T : class
    {

        /// <summary>
        /// Gets all entities that match the specified filter.
        /// </summary>
        /// <param name="filter">An expression to filter the entities.</param>
        /// <param name="includeProperties">A comma-separated list of related entities to include.</param>
        /// <returns>A list of matching entities.</returns>
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, string includeProperties = null);

        /// <summary>
        /// Gets a paged list of entities that match the specified filter.
        /// </summary>
        /// <param name="parameters">Pagination parameters.</param>
        /// <param name="filter">An expression to filter the entities.</param>
        /// <param name="includeProperties">A comma-separated list of related entities to include.</param>
        /// <returns>A paged list of matching entities.</returns>
        PagedList<T> GetAllPaged(PaginationParameters parameters, Expression<Func<T, bool>>? filter = null, string includeProperties = null);

        /// <summary>
        /// Gets a single entity that matches the specified filter.
        /// </summary>
        /// <param name="filter">An expression to filter the entity.</param>
        /// <param name="tracked">Whether the entity should be tracked by the context.</param>
        /// <param name="includeProperties">A comma-separated list of related entities to include.</param>
        /// <returns>The matching entity.</returns>
        Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true, string includeProperties = null);

        /// <summary>
        /// Checks if any entity exists that matches the specified filter.
        /// </summary>
        /// <param name="filter">An expression to filter the entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating 
        /// whether any entity exists that matches the filter.</returns>
        Task<bool> Exists(Expression<Func<T, bool>> filter);
    }
}
