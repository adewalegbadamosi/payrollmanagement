using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalaryService.Models;

namespace SalaryService.SeedDb
{
    public static class PrepDb
    {
        public static void PopulateData(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                CreateDataTable(serviceScope.ServiceProvider.GetService<ApplicationContext>(), isProd);
            }
        }

        private static void CreateDataTable(ApplicationContext context, bool isProd)
        {
 
            // if (isProd)
            // {
            //     Console.WriteLine("--> Attempting to apply migrations...");
            //     try
            //     {
            //         context.Database.Migrate();
            //     }
            //     catch (Exception ex)
            //     {
            //         Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            //     }
            // }
            if (!context.Salaries.Any())
            {
                Console.WriteLine("--> Seeding initial salary Data...");

                context.Salaries.AddRange(
                new Salary
                {
                    EmployeeId = 1,
                    Bonus = 10000,
                    DaysWorked = 20,
                    DaysAbsent = 0,
                    DailyPay = 5000,
                    GrossSalary = 103000,
                    Month = "february",
                    Year = "2023",
                    TaxDeduction = 7000
                },
                  new Salary
                  {
                      EmployeeId = 2,
                      Bonus = 10000,
                      DaysWorked = 22,
                      DaysAbsent = 0,
                      DailyPay = 4000,
                      GrossSalary = 92000,
                      Month = "February",
                      Year = "2023",
                      TaxDeduction = 6000
                  },
                  new Salary
                  {
                      EmployeeId = 2,
                      Bonus = 10000,
                      DaysWorked = 21,
                      DaysAbsent = 0,
                      DailyPay = 4000,
                      GrossSalary = 92000,
                      Month = "march",
                      Year = "2023",
                      TaxDeduction = 6000
                  }
            );
                context.SaveChanges();
            };



        }
    }
}