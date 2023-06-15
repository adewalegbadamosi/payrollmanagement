using Microsoft.AspNetCore.Mvc;
using EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeService.Interface;
using EmployeeService.Dto;
using EmployeeService.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EmployeeService.SyncDataServices.Http;

namespace EmployeeService.Repository
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        private new readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAuthenticationDataClient _httpAuthClient;

        //ApplicationContext context = new ApplicationContext();

        public EmployeeRepository(
            ApplicationContext context,
            IConfiguration configuration,
            IAuthenticationDataClient httpAuthClient,
            IMapper mapper
            )
            : base(context)
        {
            _context = context;
            _configuration = configuration;
            _httpAuthClient = httpAuthClient;
            _mapper = mapper;
        }

         public async Task<object?> GetTestUser()
        {
            // Send Sync Message
            try
            {
                var response = await _httpAuthClient.GetTestUserFromAuthenticationService();
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return null;
        }

        public async Task<EmployeeDto?> getEmployee(int id)
        {
            var employee = await _context.Employees.Select(x =>
            new EmployeeDto
            {
               Id = x.Id,
               FirstName = x.FirstName,
               LastName = x.LastName,
               Sex = x.Sex,
               Salary = x.Salary,
               Level = x.Level,
               Address = x.Address,
               EmploymentDate = x.EmploymentDate
            }).FirstOrDefaultAsync(u => u.Id == id);
            return employee;

            // var employee = await _context.Employees.FirstOrDefaultAsync(u => u.Id == id);
            // return _mapper.Map<EmployeeDto>(employee);
        }    

      
        public async Task<bool> upsertEmployee(EmployeeDto data, int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            
            if (employee != null)
            {
                employee.Address = data.Address;
                employee.EmploymentDate = data.EmploymentDate;
                employee.FirstName = data.FirstName;
                employee.LastName = data.LastName;
                employee.Level = data.Level;
                employee.Salary = data.Salary;
                employee.Sex = data.Sex;
            };

            if (_context.SaveChanges() == 1)
            {
                return true;
            };

            return false;
        }

        public async Task<bool> upsertEmployee(EmployeeDto data)
        {
            var payload = new Employee()
            {
                Address = data.Address,
                EmploymentDate = data.EmploymentDate,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Level = data.Level,
                Salary = data.Salary,
                Sex = data.Sex
            };
            await _context.Employees.AddAsync(payload);

            if (_context.SaveChanges() == 1)
            {
                return true;
            };

            return false;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployee()
        {
            // var allEmployee = await _context.Employees.Select( x =>
            // new EmployeeDto()
            // {
            //    Id = x.Id,
            //    FirstName = x.FirstName,
            //    LastName = x.LastName,
            //    Sex = x.Sex,
            //    Salary = x.Salary,
            //    Level = x.Level,
            //    Address = x.Address,
            //    EmploymentDate = x.EmploymentDate
            // }).ToListAsync();
            // return allEmployee;

            var allEmployee = await _context.Employees.ToListAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(allEmployee);

        }

        public async Task<EmployeeDto?> Delete(int id)
        {
            var response = await this.getEmployee(id);

            var employee = await _context.Employees.FirstOrDefaultAsync(u => u.Id == id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
                return response;
            }

            return null;
            //return _mapper.Map<EmployeeDto>(employee);
        }
        public async Task<bool> EmployeeExistByNames(string firstName ,string lastName)
        {         

            return await _context.Employees.AnyAsync(e => e.FirstName == firstName && e.LastName == lastName);

        }
    }
}

