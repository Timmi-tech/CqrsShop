using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration; 

public class RepositoryWriteContextFactory : IDesignTimeDbContextFactory<RepositoryWriteDbContext> 
{
    public RepositoryWriteDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();

       var builder = new DbContextOptionsBuilder<RepositoryWriteDbContext>()
            .UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                b => b.MigrationsAssembly("Infrastructure"));
        return new RepositoryWriteDbContext(builder.Options);

    }
}