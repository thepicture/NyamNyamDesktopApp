using System.Data.Entity;

namespace NyamNyamDesktopApp.Models.Factories
{
    /// <summary>
    /// Defines a method to create a DbContext.
    /// </summary>
    /// <typeparam name="TDbContext">The subclass of DbContext type which 
    /// the factory will create.</typeparam>
    public interface IDbContextFactory<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// Creates a DbContext.
        /// </summary>
        /// <returns>The subclass of DbContext.</returns>
        TDbContext Create();
    }
}
