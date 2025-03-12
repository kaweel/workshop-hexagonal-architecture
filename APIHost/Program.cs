using API.Mapping;
using API.Middleware;
using Application.Service;
using Application.Port;
using Common.Provider;
using Infrastructure.Mssql;
using Microsoft.EntityFrameworkCore;
using Controller.Adapter;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddAutoMapper(typeof(API.Mapping.MappingOrder),typeof(Application.Mapping.MappingOrder),typeof(Infrastructure.Mapping.MappingOrderEntityToDto));
        builder.Services.AddExceptionHandler<ExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddControllers().AddApplicationPart(typeof(API.Controller.OrderController).Assembly);
        builder.Services.AddDbContext<MssqlDbContext>(options =>
            options
            .UseSqlServer("Server=localhost,1433;Database=order;User Id=sa;Password=SuperStrong@Passw0rd;TrustServerCertificate=true")
            .UseLazyLoadingProxies()
        );

        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddScoped<IOrderRepositoryPort, OrderMssqlAdapter>();
        builder.Services.AddScoped<IOrderServicePort, OrderApiAdapter>();

        var app = builder.Build();

        // override config 
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            // app.UseSwagger();
            // app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseExceptionHandler();
        app.UseRouting();
        app.MapControllers();
        app.Run();
    }
}