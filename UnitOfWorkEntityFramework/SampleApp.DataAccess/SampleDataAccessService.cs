using System;
using DataAccessPatterns.Contracts;
using DataAccessPatterns.EntityFramework;

// Required in order to ensure EntityFramework.SqlServer.dll is deployed to the target folder of the executable project.
using SqlProviderServices = System.Data.Entity.SqlServer.SqlProviderServices;

namespace SampleApp.DataAccess
{
    // Provides data access support to the client side by forwarding Entity Framework support to get IUnitOfWork and IRepository<T> of the generic Entity Framework data service implementation bound to the sample database context and its entity sets.
    public class SampleDataAccessService : DataAccessService<SampleDatabaseEntities>, ISampleDataAccessService, IDataAccessService, IDisposable
    {
        // Adds support to initialize a specialized employee repository defined in the sample data access layer.
        public IEmployeeRepository GetEmployeeRepository()
        {
            return new EmployeeRepository(DbContext.Employees);
        }
    }
}
