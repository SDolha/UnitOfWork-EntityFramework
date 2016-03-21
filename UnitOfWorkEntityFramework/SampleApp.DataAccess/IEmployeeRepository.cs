using DataAccessPatterns.Contracts;
using System.Collections.Generic;

namespace SampleApp.DataAccess
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // Counts employees that are assigned to a specific department.
        int Count(Department department);

        // Gets employees that are not yet assigned to any department.
        IEnumerable<Employee> GetUnassigned();

        // Gets all employees ordered by last and first names.
        IEnumerable<Employee> GetAllOrderedByName();
    }
}