using NyamNyamDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NyamNyamDesktopApp.Services
{
    public class IngredientDataStore : IDataStore<Ingredient>
    {
        public IngredientDataStore()
        {
        }

        public Task<bool> CreateAsync(Ingredient obj)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Ingredient>> GetAllASync()
        {
            return await new NyamNyamBaseEntities().Ingredient.ToListAsync();
        }

        public Task<Ingredient> GetAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAsync(Ingredient obj, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
