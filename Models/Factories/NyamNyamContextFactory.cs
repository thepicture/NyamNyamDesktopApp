using NyamNyamDesktopApp.Models.Entities;

namespace NyamNyamDesktopApp.Models.Factories
{
    /// <summary>
    /// Implements a method to create a <see cref="NyamNyamBaseEntities"/> instance.
    /// </summary>
    public class NyamNyamContextFactory : IDbContextFactory<NyamNyamBaseEntities>
    {
        public NyamNyamBaseEntities Create()
        {
            return new NyamNyamBaseEntities();
        }
    }
}
