namespace DataAccessPatterns.Contracts
{
    // Provides data access support as a factory for IUnitOfWork and IRepository<T> implementations.
    public interface IDataAccessService
    {
        IUnitOfWork GetUnitOfWork();
        IRepository<T> GetRepository<T>() where T : class;
    }
}
