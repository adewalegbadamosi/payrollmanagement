using Microsoft.AspNetCore.Mvc;
using GatewayService.Dto;
using GatewayService.AsyncDataServices;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace GatewayService.Controllers
{
    [Route("api/v1/salaries")]
    [ApiController]
    [Authorize]
    public class SalaryApiController : ControllerBase
    {        
        private readonly IConfiguration _configuration;
        private readonly ISalaryMessageClient _salaryMessageClient;
        private  ResponseDto _response;

        public SalaryApiController(
            
            IConfiguration configuration,
         
            ISalaryMessageClient salaryMessageClient
        )
        {            
            _configuration = configuration;
            
            _salaryMessageClient = salaryMessageClient;     
           
        }
       
        [HttpGet]
        [Route("{employeeId}")]
        public async Task<IActionResult> GetEmployeeSalaries(int employeeId)
        { 
            
            var request = new
            {
                Action = "Get_Employee_Salaries",
                EmployeeId = employeeId,               
            };

            return await sendMessage(request);

        }
   
        [HttpGet]
        [Route("{employeeId}/{month}/{year}")]
        public async Task<IActionResult> GetEmployeeSalaryByPeriod(int employeeId, string month, string year)
        {   
            
            var request = new
            {
                Action = "Employee_Salary_Period",
                SalaryDetail = new SalaryDto {
                    EmployeeId = employeeId,
                    Month = month,
                    Year = year
                }
            };
            return await sendMessage(request);

        }
 
        [HttpPost]
        [Route("calculate-salary")]
        public async Task<IActionResult> CalculateSalary(ComputeDto data)
        {    
            if(data.DaysWorked < 1 || data.DaysWorked > 31)             
                return StatusCode(StatusCodes.Status400BadRequest, new { status = false, message = "Work days can only be in a range of 1 and 30" });

            var request = new
            {
                Action = "Compute_Salary",
                ComputeSalary = data
            };

            return await sendMessage(request);
        }

        private async Task<IActionResult> sendMessage (object request){
            try
            {
                var stringResponse = await _salaryMessageClient.PublishSalaryMessage(request);

                Console.WriteLine($"response {stringResponse}");
                
                _response = JsonConvert.DeserializeObject<ResponseDto>(stringResponse);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                new { status = false, message = "Asynchronous get all Salary failed" }
                );
            }

            return Ok(new
            {
                status = true,                
                data = _response.Result
            });
        }

    }
}
