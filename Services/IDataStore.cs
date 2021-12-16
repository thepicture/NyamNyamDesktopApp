using System.Collections.Generic;
using System.Threading.Tasks;

namespace NyamNyamDesktopApp.Services
{
    /// <summary>
    /// Defines asynchronous methods for CRUD
    /// operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataStore<T> where T : class
    {
        /// <summary>
        /// Gets all objects asynchronously.
        /// </summary>
        /// <returns>All objects.</returns>
        Task<IEnumerable<T>> GetAllASync();
        /// <summary>
        /// Get an object asynchronously.
        /// </summary>
        /// <param name="id">The id of an object.</param>
        /// <returns>An object.</returns>
        Task<T> GetAsync(string id);
        /// <summary>
        /// Creates a new object.
        /// </summary>
        /// <returns>A boolean representing if 
        /// creating was successful or not.</returns>
        Task<bool> CreateAsync(T obj);
        /// <summary>
        /// Updates an existing object.
        /// </summary>
        /// <param name="id">The id of an object ot update</param>
        /// <param name="obj">An object to replace with the existing object.</param>
        /// <returns>A boolean representing if 
        /// updating was successful or not.</returns>
        Task<bool> UpdateAsync(T obj, string id);
        /// <summary>
        /// Deletes an existing object.
        /// </summary>
        /// <param name="id">The id of an object.</param>
        /// <returns>A boolean representing if 
        /// deleting was successful or not.</returns>
        Task<bool> DeleteAsync(string id);
    }
}
