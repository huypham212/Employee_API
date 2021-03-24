using AppS3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<bool> SaveChange();

        Task<IEnumerable<Employee>> GetAllEmployee();

        Task<Employee> GetEmployeeById(int id);

        public Task CreateEmployee(Employee employee);

        public Task UpdateEmployee(Employee employee);

        public Task DeleteEmployee(int id);
    }
}
