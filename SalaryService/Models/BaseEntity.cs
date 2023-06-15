using System;
namespace SalaryService.Models
{
	public class BaseEntity
	{
		public BaseEntity()
		{
		}         
            public int? CreatedBy { get; set; }         
            public int? UpdatedBy { get; set; }        
            public DateTime? DateTimeCreated { get; set; }       
            public DateTime? DateTimeUpdated { get; set; }
        
    }
}

