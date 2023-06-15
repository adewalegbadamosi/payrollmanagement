using System;
namespace EmployeeService.Models
{
	public class BaseEntity
	{
		public BaseEntity()
		{
		}      
            /// <summary>
            /// User id that created the entry. Which is required at point of creating record.
            /// </summary> 
            public int? CreatedBy { get; set; }

            /// <summary>
            /// User id that last update the entry. Which is required at point of updating record.
            /// </summary>
            public int? UpdatedBy { get; set; }

            /// <summary>
            /// Datetime stamp on when record is created. The date will gotten from default constructor of the entity.
            /// </summary>
            public DateTime? DateTimeCreated { get; set; }

            /// <summary>
            /// Datetime stamp on when record was last updated. The date will gotten from the point of save changes.
            /// </summary>
            public DateTime? DateTimeUpdated { get; set; }
        
    }
}

