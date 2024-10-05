using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BankingAPI.DataAccess
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        public ApplicationDBContext CreateDbContext(string[] args)
        {
            // Load configuration from appsettings.json
            var basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "BankingAPI.WebAPI");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)  // Set base path to the correct directory
                .AddJsonFile("appsettings.json")
                .Build();

            // Create DbContextOptionsBuilder with the SQL Server provider and connection string
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDBContext(optionsBuilder.Options);
        }
    }
}
