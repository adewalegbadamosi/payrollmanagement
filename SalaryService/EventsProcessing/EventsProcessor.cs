using System;
using System.Text.Json;
using AutoMapper;
using SalaryService.Dto;
using SalaryService.Interface;
using SalaryService.Models;
using SalaryService.Repository;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;


namespace SalaryService.EventsProcessing
{
    public class EventsProcessor : IEventsProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventsProcessor(
                IServiceScopeFactory serviceScopeFactory, 
                AutoMapper.IMapper mapper                
            )
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;            
        }

        public async Task<object> ProcessSalaryEvent(string notificationMessage)

        {
            object response = null;        
            
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var salaryRepository = scope.ServiceProvider.GetRequiredService<ISalaryRepository>();                
                
                Console.WriteLine("--> Determining Event");

                var event_ = JsonConvert.DeserializeObject<GenericEventDto>(notificationMessage);

                switch(event_.Action)
                {
                    
                    case "Compute_Salary":                                                            
                        response = await salaryRepository.ComputeSalary (event_.ComputeSalary);                
                        break;
                    case "Employee_Salary_Period":                                                           
                        response = await salaryRepository.EmployeeSalaryByPeriod(
                            event_.SalaryDetail.EmployeeId,
                            event_.SalaryDetail.Month,
                            event_.SalaryDetail.Year
                            );               
                    
                        break;
                    case "Get_Employee_Salaries":  
                                                                             
                        response = await salaryRepository.EmployeeSalaries(event_.EmployeeId);                   
                        break;
                    default:
                        Console.WriteLine("--> Could not determine the event type");
                        response = EventType.Undetermined;
                        break;
                }

            }

            return response;
        }
        
    }

    enum EventType
    {
        Published = 1,
        Undetermined = 0
    }
}