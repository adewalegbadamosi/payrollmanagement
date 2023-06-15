using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalaryService.Dto
{
    public class SalaryRecordDto
    {
        public SalaryRecordDto()
        {

        }
        // public int EmployeeId { get; set; }
        public object employee{ get; set; }
        public SalaryDto salary{ get; set; }
        // public decimal Bonus { get; set; }
        // public int DaysWorked { get; set; }
        // public int DaysAbsent { get; set; }
        // public decimal DailyPay { get; set; }
        
        // public decimal GrossSalary { get; set; }  
        // public string? Month { get; set; }       
        // public string? Year { get; set; }  
        // public decimal TaxDeduction { get; set; }


    }
}



