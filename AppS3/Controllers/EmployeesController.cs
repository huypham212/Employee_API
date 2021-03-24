using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppS3.Data;
using AppS3.Models;
using AppS3.Services.EmployeeService;
using AutoMapper;
using AppS3.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace AppS3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        //[Authorize(Roles = Role.Admin)]
        ///[Authorize(Roles = Role.Employee)]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            var employee = await _service.GetAllEmployee();

            if(employee == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<EmployeeDTO>>(employee));
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        [Authorize(Roles = Role.Admin)]
        [Authorize(Roles = Role.Employee)]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById(int id)
        {
            var employee = await _service.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EmployeeDTO>(employee));
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> UpdateEmployee(int id, Employee employee)
        {
            if(employee == null)
            {
                return NoContent();
            }

            await _service.UpdateEmployee(employee);

            var result = await _service.SaveChange();

            if(result == false)
            {
                return BadRequest(new { msg = "Upadate Failed!" });
            }

            return Ok(new { msg = "Update Successful!"});
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody]Employee employee)
        {
           // var check_employee = await _service.GetEmployeeById(employee.Id);
            
            if(employee == null)
            {
                return NoContent();
            }

            //if(check_employee == null)
            //{
                await _service.CreateEmployee(employee);
                var result = await _service.SaveChange();

                if(result == false)
                {
                    return BadRequest(new { msg = "Create Failed!" });
                }

                return Ok(new {employee, msg = "Create Successful!" });
            //}

            //return Conflict();
        }

        // DELETE: api/Employees/5
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var del_employee = await _service.GetEmployeeById(id);

            if(del_employee == null)
            {
                return NotFound();
            }

            await _service.DeleteEmployee(id);
            var result = await _service.SaveChange();

            if(result == false)
            {
                return BadRequest(new { msg = "Delete Failed!" });
            }

            return Ok(new { msg = "Delete Successful!" });


        }
    }
}
