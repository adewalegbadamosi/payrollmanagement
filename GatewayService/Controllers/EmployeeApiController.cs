using Microsoft.AspNetCore.Mvc;

using GatewayService.Dto;
using GatewayService.AsyncDataServices;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using AuthenticationService.Dto;
using Microsoft.AspNetCore.Http;
using System;
using GatewayService.Helpers;

namespace GatewayService.Controllers
{
    
    [Route("api/v1/employees")]
    [ApiController]
    [Authorize]
    public class EmployeeApiController : ControllerBase
    {        
        private readonly IConfiguration _configuration;
        
        private readonly IEmployeeMessageClient _employeeMessageClient;
        private  ResponseDto _response;

        public EmployeeApiController(            
            IConfiguration configuration,
            IEmployeeMessageClient employeeMessageClient        
        )
        {            
            _configuration = configuration;
            _employeeMessageClient = employeeMessageClient;         
           
        }
       
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var currentUser =  TokenClaims.GetLoggedInUser(HttpContext); 
            //Send Async Message
             var request = new
            {
                Action = "Get_Employee",                
                EmployeeId = id,              
            };

            return await sendMessage(request);

        }      
        
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> ManageEmployee(EmployeeDto data, int id)
        {
            var currentUser =  TokenClaims.GetLoggedInUser(HttpContext); 
        
            //Send Async Message
             var request = new
            {
                Action = "Edit_Employee",
                Employee = data, 
                EmployeeId = id,              
            };

            return await sendMessage(request);
        }        
       
        [HttpPost]        
        public async Task<IActionResult> ManageEmployee(EmployeeDto data)
        {
            var currentUser =  TokenClaims.GetLoggedInUser(HttpContext);

            var request = new
            {
                Action = "Add_Employee",
                Employee = data,               
            };

            return await sendMessage(request);            
        }
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = new
            {
                Action = "Remove_Employee",
                EmployeeId = id,               
            };
              
            return await sendMessage(request);
        }
        
        [HttpGet]        
        public async Task<IActionResult> GetAll()
        {
            
            ResponseDto employees;

            var request = new
            {
                Action = "Get_Employees",                            
            };
              
            return await sendMessage(request);
        }

        private async Task<IActionResult> sendMessage (object request){
            try
            {
                var stringResponse = await _employeeMessageClient.PublishEmployeeMessage(request);

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
