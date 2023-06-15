

using SalaryService.Dto;
using SalaryService.Interfaces;
using SalaryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryService.Interface
{
    public interface ISalaryRepository :  IGenericRepository<Salary>
    {
        Task<IEnumerable<SalaryDto?>> EmployeeSalaries(int employeeId);
        Task<SalaryRecordDto?> EmployeeSalaryByPeriod(int employeeId, string month, string year);
        Task<decimal> ComputeSalary(ComputeDto data);
        Task<object> GetEmployee(int id);
        Task<object> sendMessage (object request);

    }
}
