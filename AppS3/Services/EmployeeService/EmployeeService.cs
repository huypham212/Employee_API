using AppS3.Data;
using AppS3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace AppS3.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {

        private readonly AppS3DbContext _context;

        public EmployeeService(AppS3DbContext context)
        {
            _context = context;
        }

        public async Task CreateEmployee(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public async Task DeleteEmployee(int id)
        {
            var del_employee = await _context.Employees.FirstOrDefaultAsync(key => key.Id == id);

            _context.Remove(del_employee);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployee()
        {
            var employee = await _context.Employees.ToListAsync();

            return employee;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(key => key.Id == id);

            return employee;
        }

        public async Task<bool> SaveChange()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var exist_employee = await _context.Employees.FindAsync(employee.Id);
            
            if(exist_employee != null)
            {
                exist_employee.FirstName = employee.FirstName;
                exist_employee.LastName = employee.LastName;
                exist_employee.DayGetIn = employee.DayGetIn;
                exist_employee.PositionId = employee.PositionId;
                exist_employee.SalScale1 = employee.SalScale1;
            }
        }
    }
}
