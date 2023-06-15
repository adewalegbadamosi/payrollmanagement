using EmployeeService.Dto;

namespace EmployeeService.Dto
{
    public class GenericEventDto
    {
        public string? Action { get; set; }
      
        public string? Client { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeDto? Employee { get; set; }
    }


    public class DetailDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? EmploymentDate { get; set; }
        public string? Address { get; set; }
        public string? Sex { get; set; }
       
        public string? Level { get; set; }
      
        public decimal Salary { get; set; }
    }

    
}