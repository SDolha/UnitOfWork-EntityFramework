using DataAccessPatterns.Contracts;
using System.Collections.Generic;

namespace SampleApp.DataAccess
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // Gets employees that are not yet assigned to any department.
        IEnumerable<Employee> GetUnassigned();

        // Counts employees that are assigned to a specific department.
        int Count(Department department);

        // Gets all employees ordered by last and first names.
        IEnumerable<Employee> GetAllOrderedByName();
    }
}