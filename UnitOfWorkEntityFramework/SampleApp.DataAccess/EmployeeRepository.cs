using System.Collections.Generic;
using System.Data.Entity;
using DataAccessPatterns.EntityFrameworkImplementation;

namespace SampleApp.DataAccess
{
    // On the client side you can use the generic EntityFrameworkRepository<T> implementation.
    // However, you can add specific entity logic using concrete repository implementations too.
    public class EmployeeRepository : EntityFrameworkRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDbSet<Employee> entities) : base(entities) { }

        // Get employees that are not yet assigned to any department.
        public IEnumerable<Employee> GetUnassigned()
        {
            return Get(e => e.Department == null);
        }
    }
}
