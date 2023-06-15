using AutoMapper;
using EmployeeService.Interface;
using EmployeeService.Interfaces;
using EmployeeService.Models;
using EmployeeService.Repositories;
using EmployeeService.Repository;
using EmployeeService.SeedDb;
using Microsoft.EntityFrameworkCore;
using EmployeeService.EventsProcessing;
using EmployeeService.AsyncDataServices;


var builder = WebApplication.CreateBuilder(args);

    // Database connection           
    // var conString = builder.Configuration.GetConnectionString("DbConnectionString");
    // builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(conString));

    Console.WriteLine("--> Using InMem Db");
    builder.Services.AddDbContext<ApplicationContext>(opt =>   opt.UseInMemoryDatabase("InMem"));
    //Add services to the container.

    builder.Services.AddControllers();
    // builder.Services.AddAutoMapper(typeof(ProfileMapper));
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //register these Interfaces to the respective implementaions 
    #region Repositories
    builder.Services.AddHostedService<EmployeeMessageBusServer>();
    builder.Services.AddSingleton<IEventsProcessor, EventsProcessor>(); 


    builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();

    #endregion

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.MapControllers();

    PrepDb.PrepPopulation(app, app.Environment.IsProduction());

    app.Run();

#region Repositories

#endregion

