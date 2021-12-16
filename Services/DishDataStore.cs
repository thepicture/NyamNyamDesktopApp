﻿using NyamNyamDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NyamNyamDesktopApp.Services
{
    public class DishDataStore : IDataStore<Dish>
    {
        public DishDataStore()
        {
        }

        public Task<bool> CreateAsync(Dish obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Dish>> GetAllASync()
        {
            return await new NyamNyamBaseEntities().Dish.ToListAsync();
        }

        public async Task<Dish> GetAsync(string id)
        {
            return await new NyamNyamBaseEntities().Dish.FindAsync(id);
        }

        public Task<bool> UpdateAsync(Dish obj, string id)
        {
            throw new NotImplementedException();
        }
    }
}
