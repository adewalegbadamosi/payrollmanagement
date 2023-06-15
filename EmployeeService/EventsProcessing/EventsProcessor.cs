
using AutoMapper;
using EmployeeService.Dto;
using EmployeeService.Interface;
using Newtonsoft.Json;


namespace EmployeeService.EventsProcessing
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

        public async Task<object> ProcessEvent(string notificationMessage)
        {
            object response = null;        
            
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();                
                
                Console.WriteLine("--> Determining employee Event");

                var event_ = JsonConvert.DeserializeObject<GenericEventDto>(notificationMessage);

                switch(event_.Action)
                {
                    case "Add_Employee":
                        response = await employeeRepository.upsertEmployee(event_.Employee);                  
                        break;
                    case "Edit_Employee":                 
                        response = await employeeRepository.upsertEmployee(event_.Employee, event_.EmployeeId);                 
                        break;
                    case "Remove_Employee":                                                            
                        response = await employeeRepository.Delete(event_.EmployeeId);                
                        break;
                    case "Get_Employees":                                                            
                        response = await employeeRepository.GetAllEmployee();         
                        break;
                    case "Get_Employee":                                                            
                        response = await employeeRepository.getEmployee(event_.EmployeeId);               
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