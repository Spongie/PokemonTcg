using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkingCore
{
    public static class TypeLoader
    {
        public static IEnumerable<Type> GetLoadedTypesAssignableFrom<T>()
        {
            var types = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                types.AddRange(assembly.GetTypes().Where(type => typeof(T).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract));
            }

            return types;
        }
    }
}
