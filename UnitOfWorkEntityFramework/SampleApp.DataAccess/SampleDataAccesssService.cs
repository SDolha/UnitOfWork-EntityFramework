using DataAccessPatterns.EntityFramework;
using System;

namespace SampleApp.DataAccess
{
    // Provides data access support to the client side by forwarding Entity Framework support to get IUnitOfWork and IRepository<T> of the generic Entity Framework data service implementation bound to the sample database context and its entity sets.
    public class SampleDataAccesssService : EntityFrameworkDataAccesssService<SampleDatabaseEntities>, ISampleDataAccesssService, IDisposable
    {
        // Adds support to initialize a specialized employee repository defined in the sample data access layer.
        public IEmployeeRepository GetEmployeeRepository()
        {
            return new EmployeeRepository(DbContext.Employees);
        }
    }
}
