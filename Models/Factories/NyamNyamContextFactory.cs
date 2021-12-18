using NyamNyamDesktopApp.Models.Entities;

namespace NyamNyamDesktopApp.Models.Factories
{
    public class NyamNyamContextFactory : IDbContextFactory<NyamNyamBaseEntities>
    {
        public NyamNyamBaseEntities Create()
        {
            return new NyamNyamBaseEntities();
        }
    }
}
