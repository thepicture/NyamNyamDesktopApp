using NyamNyamDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NyamNyamDesktopApp.Services
{
    public class DishCategoryDataStore : IDataStore<DishCategory>
    {
        public DishCategoryDataStore()
        {
        }

        public Task<bool> CreateAsync(DishCategory obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DishCategory>> GetAllASync()
        {
            return await new NyamNyamBaseEntities().DishCategory.ToListAsync();
        }

        public Task<DishCategory> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(DishCategory obj, string id)
        {
            throw new NotImplementedException();
        }
    }
}
