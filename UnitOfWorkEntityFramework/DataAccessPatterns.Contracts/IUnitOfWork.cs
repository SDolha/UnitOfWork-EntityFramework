namespace DataAccessPatterns.Contracts
{
    // Generic interface for unit of work pattern.
    public interface IUnitOfWork
    {
        // Change notification methods.
        void RegisterNew(object entity);
        void RegisterDirty(object entity);
        void RegisterClean(object entity);
        void RegisterDeleted(object entity);

        // Commit and rollback methods.
        void Commit();
        void Rollback();
    }
}
