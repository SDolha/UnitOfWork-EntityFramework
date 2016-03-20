using System.Collections.Generic;
using System.Data.Entity;
using DataAccessPatterns.EntityFramework;
using System.Linq;

namespace SampleApp.DataAccess
{
    // Besides having the generic EntityFrameworkRepository<T> implementation, you can add specific entity logic using specialized repository implementations.
    public class EmployeeRepository : EntityFrameworkRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDbSet<Employee> employees) : base(employees) { }

        // Get employees that are not yet assigned to any department.
        public IEnumerable<Employee> GetUnassigned()
        {
            // Using the get method of the base repository implementation.
            return Get(e => e.Department == null);
        }

        // Get employees ordered by last and first names.
        public IEnumerable<Employee> GetAllOrderedByName()
        {
            // Using the entity set of the base repository implementation and its protected item collection provider.
            return GetItemCollection(Entities.OrderBy(e => e.LastName).ThenBy(e => e.FirstName));
        }
    }
}
