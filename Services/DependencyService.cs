using System;
using System.Collections.Generic;
using System.Linq;

namespace NyamNyamDesktopApp.Services
{
    public static class DependencyService
    {
        private static readonly Dictionary<Type, object> _implementations =
            new Dictionary<Type, object>();

        public static void Register<T>()
        {
            _implementations.Add(typeof(T), Activator.CreateInstance(typeof(T)));
        }

        public static T Get<T>()
        {
            object implementation = _implementations.First(i =>
            {
                return typeof(T).IsAssignableFrom(i.Value.GetType());
            }).Value;
            return implementation == null
                ? throw new Exception("No implementation found " +
                "for type " + typeof(T).Name)
                : (T)implementation;
        }
    }
}
