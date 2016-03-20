namespace DataAccessPatterns.Contracts
{
    /// <summary>
    /// Generic interface for unit of work pattern.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Notifies the unit of work object that the specified entity is new and should actually be added to the data store upon committing the changes.
        /// </summary>
        void RegisterNew(object entity);

        /// <summary>
        /// Notifies the unit of work object that the specified entity is modified and should actually be updated in the data store upon committing the changes.
        /// </summary>
        void RegisterDirty(object entity);

        /// <summary>
        /// Notifies the unit of work object that the specified entity is not modified and should be preserved as is in the data store upon committing the changes.
        /// </summary>
        void RegisterClean(object entity);

        /// <summary>
        /// Notifies the unit of work object that the specified entity is deleted and should actually be removed from the data store upon committing the changes.
        /// </summary>
        void RegisterDeleted(object entity);

        /// <summary>
        /// Commits changes registered on the unit of work object by performing actual insert, update, and delete operations on the data store.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back th unit of work object to its initial state.
        /// </summary>
        void Rollback();
    }
}
