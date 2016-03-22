using System.Collections.Generic;
using DataAccessPatterns.Contracts;

namespace SampleApp.DataAccess
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // Gets all employees ordered by last and first names.
        IEnumerable<Employee> GetAllOrderedByName();

        // Counts employees that are assigned to a specific department.
        int Count(Department department);

        // Counts employees that are not yet assigned to any department.
        int CountUnassigned();
    }
}