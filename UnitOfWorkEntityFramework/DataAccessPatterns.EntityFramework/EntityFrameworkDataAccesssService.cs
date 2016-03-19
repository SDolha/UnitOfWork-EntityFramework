using DataAccessPatterns.Contracts;
using System;
using System.Data.Entity;

namespace DataAccessPatterns.EntityFramework
{
    // Provides data access support by passing Entity Framework implementations of IUnitOfWork and IRepository<T> bound to an instance of the specified database context type and its entity sets.
    // You can then always switch to a different data access technology by changing this mapping provided that you have IUnitOfWork and IRepository<T> implementations for the other technology ready, and the client side will not need to change.
    // Data object types (used by the database context) can be POCOs so Entity Framework could be entirely replaced with another system.
    public class EntityFrameworkDataAccesssService<ContextType> : IDataAccesssService, IDisposable where ContextType : DbContext
    {
        public EntityFrameworkDataAccesssService(ContextType dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            DbContext = dbContext;
        }

        protected ContextType DbContext { get; private set; }

        // Initializes an Entity Framwork unit of work instance bound to the database context.
        public IUnitOfWork GetUnitOfWork()
        {
            return new EntityFrameworkUnitOfWork(DbContext);
        }
        // Initializes an Entity Framework repository instance bound to an entity set from the database context, as indicated by the generic type argument of the method call.
        public IRepository<T> GetRepository<T>() where T : class
        {
            var dbSet = DbContext.Set<T>();
            return new EntityFrameworkRepository<T>(dbSet);
        }

        #region Disposing

        private bool hasBeenDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (hasBeenDisposed)
                return;
            if (disposing)
                DbContext.Dispose();
            hasBeenDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
        
        #endregion
    }
}
