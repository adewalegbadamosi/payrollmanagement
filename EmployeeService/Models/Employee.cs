using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeService.Models
{
	public class Employee : BaseEntity
	{
        //public TblEmployeeRecord()
        //{
        //}

        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? EmploymentDate { get; set; }
        public string? Address { get; set; }
        public string? Sex { get; set; }
        [Required]
        public string? Level { get; set; }
        [Required]
        public decimal Salary { get; set; } 
        public string? Email { get; set; } 
        public DateTime? DateOfBirth { get; set; } 
        public string? Qualification { get; set; }      
        
    }

    public class Punctuality {
        public int EmployeeId { get; set;} 
        public int Present { get; set; }
        public int Absent { get; set; }
        public int excused { get; set; }
        public DateTime Period { get; set; }
    }

    public enum Bonus {
        Punctuality = 10000,
        Performance = 20000
    }
}


