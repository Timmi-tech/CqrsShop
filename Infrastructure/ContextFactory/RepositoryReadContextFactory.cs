// using Infrastructure.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration; 

// public class RepositoryReadContextFactory : IDesignTimeDbContextFactory<RepositoryReadDbContext> 
// {
//     public RepositoryReadDbContext CreateDbContext(string[] args)
//     {
//         var configuration = new ConfigurationBuilder()
//             .SetBasePath(Directory.GetCurrentDirectory())
//             .AddJsonFile("appsettings.Development.json")
//             .Build();

//        var builder = new DbContextOptionsBuilder<RepositoryReadDbContext>()
//             .UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
//                 b => b.MigrationsAssembly("Infrastructure"));
//         return new RepositoryReadDbContext(builder.Options);

//     }
// }