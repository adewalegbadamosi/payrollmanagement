using Microsoft.AspNetCore.Mvc;
using SalaryService.Models;
using SalaryService.AsyncDataServices;
using SalaryService.Interface;
using SalaryService.Dto;
using SalaryService.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using GatewayService.Dto;
using Newtonsoft.Json;

namespace SalaryService.Repository
{
    public class SalaryRepository : GenericRepository<Salary> , ISalaryRepository
    {
        private new readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmployeeMessageClient _employeeMessageClient;
        private  ResponseDto _response;


        public SalaryRepository(
            ApplicationContext context,
            IConfiguration configuration,
            IEmployeeMessageClient employeeMessageClient,      
            IMapper mapper) : base(context)

        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _employeeMessageClient = employeeMessageClient;         

        }
        public async Task<IEnumerable<SalaryDto?>> EmployeeSalaries(int employeeId)
        {          
            var employeeSalaries = await _context.Salaries.Where(e => e.EmployeeId == employeeId).ToListAsync();
            return _mapper.Map<IEnumerable<SalaryDto>>(employeeSalaries);

        }  
        public async Task<SalaryRecordDto?> EmployeeSalaryByPeriod(int employeeId, string month, string year)
        {
            var Salary = await _context.Salaries.FirstOrDefaultAsync(u => 
                u.EmployeeId == employeeId && u.Year == year && u.Month.ToLower() == month.ToLower()
            );
            var employee = await GetEmployee(employeeId);
            var salary = _mapper.Map<SalaryDto>(Salary);

            return new SalaryRecordDto{
                employee = employee,
                salary = salary
            };
        }      
        public async Task<decimal> ComputeSalary(ComputeDto data)
        {
            decimal computedSalary;            
            var currentYear = DateTime.Now.Year.ToString();            
            
            var periodSalary = await EmployeeSalaryByPeriod(data.EmployeeId, data.Month, data.Year) ;
       
            if (periodSalary != null){
                return periodSalary.salary.GrossSalary;
            }else {
                computedSalary = data.DailyPay * data.DaysWorked + data.Bonus - data.TaxDeduction;
                
                var salaryDetails = _mapper.Map<Salary>(data);
                salaryDetails.GrossSalary = computedSalary;

                await _context.Salaries.AddAsync(salaryDetails);
                if (_context.SaveChanges() == 1) return computedSalary;              
            };            

            return computedSalary;
        }
        public async Task<object> GetEmployee(int id)
        {            
            //Send Async Message
             var request = new
            {
                Action = "Get_Employee",                
                EmployeeId = id,              
            };

            return await sendMessage(request);
        } 
        public async Task<object> sendMessage (object request)
        {
            try
            {
                var stringResponse = await _employeeMessageClient.PublishEmployeeMessage(request);

                Console.WriteLine($"response {stringResponse}");
                
                _response = JsonConvert.DeserializeObject<ResponseDto>(stringResponse);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
                return  null;
            }

            return _response.Result;
        }   

    }
}

