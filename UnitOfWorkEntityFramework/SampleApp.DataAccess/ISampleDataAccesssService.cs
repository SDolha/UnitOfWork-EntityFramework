using DataAccessPatterns.Contracts;
using System;

namespace SampleApp.DataAccess
{
    public interface ISampleDataAccesssService : IDisposable
    {
        IUnitOfWork GetUnitOfWork();
        IRepository<T> GetRepository<T>() where T : class;
        IEmployeeRepository GetEmployeeRepository();
    }
}
