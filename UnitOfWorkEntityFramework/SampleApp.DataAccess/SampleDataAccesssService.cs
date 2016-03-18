using DataAccessPatterns.Contracts;
using DataAccessPatterns.EntityFrameworkImplementation;
using System;

namespace SampleApp.DataAccess
{
    // Provides data access support to the client side by passing Entity Framework implementations  of IUnitOfWork and IRepsotory<T> bound to the sample database context and its entity sets.
    // You can then always switch to a different data access technology by changing this mapping provided that you have IUnitOfWork and IRepository<T> implementations for the other technology ready, and the client side will not need to change.
    // Data object types (such as Department and Employee) can be POCOs so Entity Framework could be entirely replaced with another system.
    public class SampleDataAccesssService : ISampleDataAccesssService, IDisposable
    {
        private SampleDatabaseEntities sampleDbContext;
        public SampleDataAccesssService()
        {
            sampleDbContext = new SampleDatabaseEntities();
        }

        // Initializes an Entity Framwork unit of work instance bound to the sample database context.
        public IUnitOfWork GetUnitOfWork()
        {
            return new EntityFrameworkUnitOfWork(sampleDbContext);
        }
        // Initializes an Entity Framework repository instance bound to an entity set from the sample database context, as indicated by the generic type argument of the method call.
        public IRepository<T> GetRepository<T>() where T : class
        {
            var dbSet = sampleDbContext.Set<T>();
            return new EntityFrameworkRepository<T>(dbSet);
        }
        // Initializes a specialized employee repository also defined in the sample data access layer.
        public IEmployeeRepository GetEmployeeRepository()
        {
            return new EmployeeRepository(sampleDbContext.Employees);
        }

        #region Disposing

        private bool hasBeenDisposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (hasBeenDisposed)
                return;
            if (disposing)
                sampleDbContext.Dispose();
            hasBeenDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
        
        #endregion
    }
}
