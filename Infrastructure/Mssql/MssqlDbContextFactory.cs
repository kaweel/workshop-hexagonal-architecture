using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Mssql;
public class MssqlDbContextFactory : IDesignTimeDbContextFactory<MssqlDbContext>
{
    public MssqlDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MssqlDbContext>()
        .UseSqlServer("Server=localhost,1433;Database=order;User Id=sa;Password=SuperStrong@Passw0rd;TrustServerCertificate=true")
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .Options;
        return new MssqlDbContext(optionsBuilder);
    }
}
