using System;
using System.Linq;
using DataAccessPatterns.Contracts;
using SampleApp.DataAccess;

namespace SampleApp.UserInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            // Using a disposable data access service instance. Dependency injection can also be used to obtain the actual instance to use.
            using (ISampleDataAccessService sampleDataAccessService = new SampleDataAccessService())
            {
                // Initialize unit of work and repository implementations.
                IUnitOfWork unitOfWork = sampleDataAccessService.GetUnitOfWork();
                // You can use either generic type or specialized repository instances.
                IRepository<Department> departmentRepository = sampleDataAccessService.GetRepository<Department>();
                IEmployeeRepository employeeRepository = sampleDataAccessService.GetEmployeeRepository();

                ExecuteClientActions(unitOfWork, departmentRepository, employeeRepository);
            }
        }

        // Client side actions are executed calling data access pattern interfaces.
        private static void ExecuteClientActions(IUnitOfWork unitOfWork, IRepository<Department> departmentRepository, IEmployeeRepository employeeRepository)
        {
            // Include calls are supported by EntityFramework package, but DataAccessPatterns.EntityFramework provides an including params argument for convenience as well.
            var departments = departmentRepository.Get(items => items.OrderBy(d => d.Name), d => d.Employees);
            Console.WriteLine($"Initially there are {departments.Count} departments:");
            foreach (var department in departments)
            {
                Console.WriteLine($" - {department.Name} with {department.Employees.Count} employees:");
                foreach (var employee in department.Employees)
                    Console.WriteLine($"   - {employee.FirstName} {employee.LastName}");
            }

            var developmentDepartment = departmentRepository.Get(items => items.Where(d => d.Name == "Development").Single());
            var developerCount = employeeRepository.Count(developmentDepartment);
            var johnEmployeeCount = employeeRepository.Count(items => items.Where(e => e.FirstName == "John"));
            var unassignedCount = employeeRepository.CountUnassigned();
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

            developerCount = employeeRepository.Count(developmentDepartment);
            johnEmployeeCount = employeeRepository.Count(items => items.Where(e => e.FirstName == "John"));
            unassignedCount = employeeRepository.CountUnassigned();
            Console.WriteLine($"Now there are {developerCount} developers.");
            Console.WriteLine($"There are {johnEmployeeCount} employees named John.");
            Console.WriteLine($"{unassignedCount} employees are not yet assigned to a department.");

            newUnassignedEmployee.FirstName = "Johnny";
            newUnassignedEmployee.Department = developmentDepartment;
            unitOfWork.RegisterDirty(newUnassignedEmployee);
            unitOfWork.Commit();
            Console.WriteLine("Employee John Spencer changed his first name to Johnny and became developer.");

            johnEmployeeCount = employeeRepository.Count(items => items.Where(e => e.FirstName == "John"));
            unassignedCount = employeeRepository.CountUnassigned();
            Console.WriteLine($"Now there are {johnEmployeeCount} employees named John.");
            Console.WriteLine($"{unassignedCount} employees are still not yet assigned to a department.");

            Console.WriteLine("All employees ordered by name are:");
            foreach (var employee in employeeRepository.GetAllOrderedByName())
                Console.WriteLine($" - {employee.LastName}, {employee.FirstName}");
        }
    }
}
