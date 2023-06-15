

using EmployeeService.Dto;
using EmployeeService.Interfaces;
using EmployeeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.Interface
{
    public interface IEmployeeRepository :  IGenericRepository<Employee>
    {        
        Task<EmployeeDto?> getEmployee(int id);

        Task<IEnumerable<EmployeeDto>> GetAllEmployee();
       
        Task<bool> upsertEmployee(EmployeeDto data);

        Task<bool> upsertEmployee(EmployeeDto data, int id);

        Task<EmployeeDto?> Delete(int id);
        Task<bool> EmployeeExistByNames(string firstName ,string lastName);


    }
}
