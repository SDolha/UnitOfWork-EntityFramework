using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataAccessPatterns.EntityFramework;

namespace SampleApp.DataAccess
{
    // Besides having the generic EntityFrameworkRepository<T> implementation, you can add specific entity logic using specialized repository implementations.
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDbSet<Employee> entities) : base(entities) { }

        // Gets all employees ordered by last and first names.
        public IEnumerable<Employee> GetAllOrderedByName()
        {
            return Get(items => items.OrderBy(e => e.LastName).ThenBy(e => e.FirstName));
        }

        // Counts employees that are assigned to a specific department.
        public int Count(Department department)
        {
            return Count(items => items.Where(e => e.DepartmentId == department.Id));
        }

        // Counts employees that are not yet assigned to any department.
        public int CountUnassigned()
        {
            return Count(items => items.Where(e => e.DepartmentId == null));
        }
    }
}
