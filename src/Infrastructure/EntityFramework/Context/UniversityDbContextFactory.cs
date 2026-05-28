using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.EntityFramework.Context;

public class UniversityDbContextFactory : IDesignTimeDbContextFactory<UniversityDbContext>
{
    public UniversityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UniversityDbContext>();
        var databasePath = Path.Combine(Directory.GetCurrentDirectory(), UniversityDbContext.DatabaseFileName);

        optionsBuilder.UseSqlite($"Data Source={databasePath}");

        return new UniversityDbContext(optionsBuilder.Options);
    }
}
