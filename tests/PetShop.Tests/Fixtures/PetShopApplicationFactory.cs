using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PetShop.Data;
using PetShop.DbContexts;
using System.Data.Common;

namespace PetShop.Tests.Fixtures;

public class PetShopApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection = new SqliteConnection("Data Source=:memory:");

    public HttpClient HttpClient { get; }

    public PetShopApplicationFactory()
    {
        HttpClient = CreateClient();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connection.Open();

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<PetShopDbContext>));
            services.RemoveAll(typeof(PetShopDbContext));

            services.AddDbContext<PetShopDbContext>(options =>
                options.UseSqlite(_connection));
        });
    }

    public async Task CreateDatabase()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PetShopDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        var seedingService = new SeedingService(context);
        await seedingService.SeedAsync();
    }

    public async Task DeleteDatabase()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PetShopDbContext>();

        await context.Database.EnsureDeletedAsync();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }

        base.Dispose(disposing);
    }
}
