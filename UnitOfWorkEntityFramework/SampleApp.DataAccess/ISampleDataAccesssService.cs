using DataAccessPatterns.Contracts;
using System;

namespace SampleApp.DataAccess
{
    public interface ISampleDataAccesssService : IDataAccessService, IDisposable
    {
        IEmployeeRepository GetEmployeeRepository();
    }
}
