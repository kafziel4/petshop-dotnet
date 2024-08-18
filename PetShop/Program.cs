using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.DbContexts;
using PetShop.Extensions;
using Serilog;
using Serilog.Formatting.Compact;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new CompactJsonFormatter())
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddSerilog();

    builder.Services.AddDbContext<PetShopDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("PetShopDB")));

    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    builder.Services.AddProblemDetails();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging();

    app.UseSwagger();
    app.UseSwaggerUI();

    if (app.Environment.IsDevelopment())
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<PetShopDbContext>();

        await context.Database.MigrateAsync();

        var seedingService = new SeedingService(context);
        await seedingService.SeedAsync();
    }

    app.UseHttpsRedirection();

    app.RegisterProductsEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program;
