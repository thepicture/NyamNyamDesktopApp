using System.Data.Entity;

namespace NyamNyamDesktopApp.Models.Factories
{
    public interface IDbContextFactory<TDbContext> where TDbContext : DbContext
    {
        TDbContext Create();
    }
}
