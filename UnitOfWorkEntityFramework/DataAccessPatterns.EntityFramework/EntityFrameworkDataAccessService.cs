using DataAccessPatterns.Contracts;
using System;
using System.Data.Entity;

namespace DataAccessPatterns.EntityFramework
{
    /// <summary>
    /// Provides data access support by passing Entity Framework implementations of <see cref="IUnitOfWork"/> and <see cref="IRepository{T}"/> bound to an instance of the specified database context type and its entity sets.
    /// You can then always switch to a different data access technology by changing this mapping provided that you have <see cref="IUnitOfWork"/> and <see cref="IRepository{T}"/> for the other technology ready, and the client side will not need to change.
    /// Data object types (used by the database context) can be POCOs so Entity Framework could be entirely replaced with another system.
    /// </summary>
    /// <typeparam name="TDbContext">DbContext type</typeparam>
    public class EntityFrameworkDataAccessService<TDbContext> : IDataAccessService, IDisposable where TDbContext : DbContext, new()
    {
        public EntityFrameworkDataAccessService()
        {
            DbContext = new TDbContext();
        }

        protected TDbContext DbContext { get; private set; }

        /// <summary>
        /// Initializes an Entity Framwork unit of work instance bound to the database context.
        /// </summary>
        public IUnitOfWork GetUnitOfWork()
        {
            return new EntityFrameworkUnitOfWork(DbContext);
        }
        /// <summary>
        /// Initializes an Entity Framework repository instance bound to an entity set from the database context, as indicated by the generic type argument of the method call.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
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
