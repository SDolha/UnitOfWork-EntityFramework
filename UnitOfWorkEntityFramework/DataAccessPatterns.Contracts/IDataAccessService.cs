namespace DataAccessPatterns.Contracts
{
    /// <summary>
    /// Provides data access support as a factory for <see cref="IUnitOfWork"/> and <see cref="IRepository{T}"/> implementations.
    /// </summary>
    public interface IDataAccessService
    {
        /// <summary>
        /// Gets a unit of work instance.
        /// </summary>
        IUnitOfWork GetUnitOfWork();

        /// <summary>
        /// Gets a repository instance for entities of the specified type.
        /// </summary>
        IRepository<T> GetRepository<T>() where T : class;
    }
}
