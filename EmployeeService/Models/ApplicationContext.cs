using System;
using EmployeeService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Models
{
	public class ApplicationContext : DbContext
    {
		public ApplicationContext()
		{
		}

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }

   
    }






    }



