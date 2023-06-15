using AutoMapper;
using SalaryService.Interface;
using SalaryService.Interfaces;
using SalaryService.Models;
using SalaryService.Repositories;
using SalaryService.Repository;
using SalaryService.SeedDb;
using Microsoft.EntityFrameworkCore;
using SalaryService.Mapper;
using SalaryService.EventsProcessing;
using SalaryService.AsyncDataServices;


var builder = WebApplication.CreateBuilder(args);

        // Database connection           
        // var conString = builder.Configuration.GetConnectionString("DbConnectionString");
        // builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(conString));

        Console.WriteLine("--> Using InMem Db");
        builder.Services.AddDbContext<ApplicationContext>(opt =>   opt.UseInMemoryDatabase("SalaryDb"));
        //Add services to the container.

        builder.Services.AddControllers();        
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //register these Interfaces to the respective implementaions 
        #region Repositories
        builder.Services.AddHostedService<MessageBusServer>();
        builder.Services.AddSingleton<IEventsProcessor, EventsProcessor>(); 
   

        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddTransient<ISalaryRepository, SalaryRepository>();
        builder.Services.AddSingleton<IEmployeeMessageClient, EmployeeMessageClient>();
        


        #endregion

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        PrepDb.PopulateData(app, app.Environment.IsProduction());

        app.Run();


#region Repositories

#endregion

