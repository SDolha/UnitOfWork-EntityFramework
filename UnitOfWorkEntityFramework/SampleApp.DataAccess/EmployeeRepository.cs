using System.Collections.Generic;
using System.Data.Entity;
using DataAccessPatterns.EntityFrameworkImplementation;

namespace SampleApp.DataAccess
{
    // Besides having the generic EntityFrameworkRepository<T> implementation, you can add specific entity logic using specialized repository implementations.
    public class EmployeeRepository : EntityFrameworkRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDbSet<Employee> employees) : base(employees) { }

        // Get employees that are not yet assigned to any department.
        public IEnumerable<Employee> GetUnassigned()
        {
            return Get(e => e.Department == null);
        }
    }
}
