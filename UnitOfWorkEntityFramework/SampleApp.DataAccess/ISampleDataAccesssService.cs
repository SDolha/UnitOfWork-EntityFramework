using DataAccessPatterns.Contracts;
using System;

namespace SampleApp.DataAccess
{
    public interface ISampleDataAccesssService : IDisposable
    {
        IEmployeeRepository GetEmployeeRepository();
        IRepository<T> GetRepository<T>() where T : class;
        IUnitOfWork GetUnitOfWork();
    }
}