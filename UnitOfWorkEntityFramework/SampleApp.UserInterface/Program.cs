using System;
using System.Linq;
using SampleApp.DataAccess;
using DataAccessPatterns.EntityFrameworkImplementation;
using DataAccessPatterns.Contracts;

namespace SampleApp.UserInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            // Using a disposable Entity Framework database context instance.
            using (var sampleDbContext = new SampleDatabaseEntities())
            {
                // Initialize unit of work and repository implementations over the database context and its database entity sets.
                // Of course, you can use a dependency injection framework to resolve IUnitOfWork, IRepository<T> or specialized interfaces instead.
                // You can then always switch to a different data access technology provided that you implement the same interfaces.
                var unitOfWork = new EntityFrameworkUnitOfWork(sampleDbContext);
                // You can use either generic type or specialized repository instances.
                var departmentRepository = new EntityFrameworkRepository<Department>(sampleDbContext.Departments);
                var employeeRepository = new EmployeeRepository(sampleDbContext.Employees);

                // Perform client side actions using the unit of work and repository instances.
                ExecuteClientActions(unitOfWork, departmentRepository, employeeRepository);
            }
        }

        // Client side actions work through data access pattern contracts (interfaces).
        // Data object types (such as Department and Employee) can be POCOs so Entity Framework could be entirely replaced with another system.
        private static void ExecuteClientActions(IUnitOfWork unitOfWork, IRepository<Department> departmentRepository, IEmployeeRepository employeeRepository)
        {
            var developmentDepartment = departmentRepository.GetSingle(d => d.Name == "Development");
            var developerCount = developmentDepartment.Employees.Count();
            var johnEmployeeCount = employeeRepository.Get(e => e.FirstName == "John").Count();
            var unassignedCount = employeeRepository.GetUnassigned().Count();
            Console.WriteLine($"Initially there are {developerCount} developers.");
            Console.WriteLine($"There are {johnEmployeeCount} employees named John.");
            Console.WriteLine($"{unassignedCount} employees are not yet assigned to a department.");

            var newDeveloper = new Employee { FirstName = "John", LastName = "Daniels", Department = developmentDepartment };
            employeeRepository.Add(newDeveloper);
            // Remember to add UnitOfWork.Register* calls whenever the logic requires it, regardless of the fact that Entity Framework unit of work implementation does nothing on these calls.
            unitOfWork.RegisterNew(newDeveloper);
            var newUnassignedEmployee = new Employee { FirstName = "John", LastName = "Spencer" };
            employeeRepository.Add(newUnassignedEmployee);
            unitOfWork.RegisterNew(newUnassignedEmployee);
            unitOfWork.Commit();
            Console.WriteLine("John Daniels (developer) and John Spencer (not yet assigned) have been added.");

            developerCount = developmentDepartment.Employees.Count();
            johnEmployeeCount = employeeRepository.Get(e => e.FirstName == "John").Count();
            unassignedCount = employeeRepository.GetUnassigned().Count();
            Console.WriteLine($"Now there are {developerCount} developers.");
            Console.WriteLine($"There are {johnEmployeeCount} employees named John.");
            Console.WriteLine($"{unassignedCount} employees are not yet assigned to a department.");

            newUnassignedEmployee.FirstName = "Johnny";
            newUnassignedEmployee.Department = developmentDepartment;
            unitOfWork.RegisterDirty(newUnassignedEmployee);
            unitOfWork.Commit();
            Console.WriteLine("Employee John Spencer changed his first name to Johnny and became developer.");

            johnEmployeeCount = employeeRepository.Get(e => e.FirstName == "John").Count();
            unassignedCount = employeeRepository.GetUnassigned().Count();
            Console.WriteLine($"Now there are {johnEmployeeCount} employees named John.");
            Console.WriteLine($"{unassignedCount} employees are still not yet assigned to a department.");
        }
    }
}
