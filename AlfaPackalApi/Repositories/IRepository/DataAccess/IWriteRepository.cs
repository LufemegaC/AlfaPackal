namespace Api_PACsServer.Repositories.IRepository.DataAccess
{
    /// <summary>
    /// Represents a repository interface for creating and saving entities.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <author>Luis F. Méndez G.</author>
    public interface IWriteRepository<T> where T : class
    {
        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>A task that represents the asynchronous create operation.</returns>
        Task Create(T entity);
        /// <summary>
        /// Saves all changes made in the context.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task Save();
    }
}
