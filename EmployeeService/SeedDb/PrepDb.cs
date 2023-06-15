using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EmployeeService.Models;

namespace EmployeeService.SeedDb
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationContext>(), isProd);
            }
        }

        private static void SeedData(ApplicationContext context, bool isProd)
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

            if (!context.Employees.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Employees.AddRange(
                    new Employee()
                    {
                        Id = 1,
                        FirstName = "Shusan",
                        LastName = "Dorcas",
                        Sex = "Female",
                        Salary = 120000,
                        Level = "Senior",
                        Address = "Moniya ",
                        EmploymentDate = new DateTime()
                    },
                    new Employee()
                    {

                        Id = 2,
                        FirstName = "Toba",
                        LastName = "Dorcas",
                        Sex = "Male",
                        Salary = 120000,
                        Level = "Senior",
                        Address = "San Andreas",
                        EmploymentDate = new DateTime()
                    },
                    new Employee()
                    {

                        Id = 3,
                        FirstName = "Sandra",
                        LastName = "Dorcas",
                        Sex = "Female",
                        Salary = 300000,
                        Level = "Junior",
                        Address = "Lagos",
                        EmploymentDate = new DateTime()
                    }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}