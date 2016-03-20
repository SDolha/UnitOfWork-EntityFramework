using DataAccessPatterns.Contracts;
using System;

namespace SampleApp.DataAccess
{
    public interface ISampleDataAccessService : IDataAccessService, IDisposable
    {
        IEmployeeRepository GetEmployeeRepository();
    }
}
