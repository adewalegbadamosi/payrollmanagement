using System;
using SalaryService.Models;
using Microsoft.EntityFrameworkCore;

namespace SalaryService.Models
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

        public virtual DbSet<Salary> Salaries { get; set; }

       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configuration.GetConnectionString("DbConnectionString");
            optionsBuilder.UseSqlServer(connectionString); 
        }
    }

   
    }






    }



