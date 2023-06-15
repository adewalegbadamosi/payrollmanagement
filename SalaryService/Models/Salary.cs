﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalaryService.Models
{
	public class Salary : BaseEntity
	{
   
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal Bonus { get; set; }
        public int DaysWorked { get; set; }
        public int DaysAbsent { get; set; }
        public decimal DailyPay { get; set; }
        public decimal GrossSalary { get; set; }       
        public string? Month { get; set; }       
        public string? Year { get; set; }   
        public decimal TaxDeduction { get; set; }

        
    }
}


