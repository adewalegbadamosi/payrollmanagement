using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GatewayService.Dto
{
    public class ComputeDto
    {
        public ComputeDto()
        {

        }
        public int EmployeeId { get; set; }
        public decimal Bonus { get; set; }
        public int DaysWorked { get; set; }
        // public int DaysAbsent { get; set; }
        public decimal DailyPay { get; set; }
      
        public string? month { get; set; }       
        public string? year { get; set; }  
        public decimal TaxDeduction { get; set; }


    }
}



