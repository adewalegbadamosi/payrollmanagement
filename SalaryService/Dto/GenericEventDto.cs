

namespace SalaryService.Dto
{
    public class GenericEventDto
    {
        public string? Action { get; set; }    
       
        public int SalaryId { get; set; }
        public int EmployeeId { get; set; }
        public SalaryDto? SalaryDetail { get; set; }
        public ComputeDto? ComputeSalary { get; set; }
    }



    
}