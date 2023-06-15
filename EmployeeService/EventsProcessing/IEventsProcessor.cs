namespace EmployeeService.EventsProcessing
{
    public interface IEventsProcessor
    {
        // object ProcessEvent(string message);
        Task<object> ProcessEvent(string notifcationMessage);

    }
}