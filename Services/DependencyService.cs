using System;
using System.Collections.Generic;
using System.Linq;

namespace NyamNyamDesktopApp.Services
{
    /// <summary>
    /// Implements methods for registering 
    /// and getting implementations of abstractions.
    /// </summary>
    public static class DependencyService
    {
        private static readonly Dictionary<Type, object> _implementations =
            new Dictionary<Type, object>();

        /// <summary>
        /// Registers an implementation type of an abstraction.
        /// </summary>
        /// <typeparam name="T">The implementation type of an asbtraction.</typeparam>
        public static void Register<T>()
        {
            _implementations.Add(typeof(T), Activator.CreateInstance(typeof(T)));
        }

        /// <summary>
        /// Gets an implementation type of an abstraction 
        /// by specifying the inteface.
        /// </summary>
        /// <exception cref="ApplicationException">
        /// Throws when no implementation found.
        /// </exception>
        /// <typeparam name="T">The interface type.</typeparam>
        /// <returns>The implementation type.</returns>
        public static T Get<T>()
        {
            object implementation = _implementations.First(i =>
            {
                return typeof(T).IsAssignableFrom(i.Value.GetType());
            }).Value;
            return implementation == null
                ? throw new ApplicationException("No implementation found " +
                "for type " + typeof(T).Name)
                : (T)implementation;
        }
    }
}
