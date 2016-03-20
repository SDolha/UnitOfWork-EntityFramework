using DataAccessPatterns.Contracts;
using System.Collections.Generic;

namespace SampleApp.DataAccess
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IEnumerable<Employee> GetUnassigned();
        IEnumerable<Employee> GetAllOrderedByName();
    }
}