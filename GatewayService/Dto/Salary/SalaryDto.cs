using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GatewayService.Dto
{
    public class SalaryDto
    {
        public SalaryDto()
        {

        }
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



