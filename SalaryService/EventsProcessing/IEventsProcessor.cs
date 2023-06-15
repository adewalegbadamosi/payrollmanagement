namespace SalaryService.EventsProcessing
{
    public interface IEventsProcessor
    {       
        Task<object> ProcessSalaryEvent(string notifcationMessage);

    }
}