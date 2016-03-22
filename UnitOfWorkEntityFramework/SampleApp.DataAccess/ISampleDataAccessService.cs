using System;
using DataAccessPatterns.Contracts;

namespace SampleApp.DataAccess
{
    public interface ISampleDataAccessService : IDataAccessService, IDisposable
    {
        IEmployeeRepository GetEmployeeRepository();
    }
}
