using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkingCore
{
    public static class TypeLoader
    {
        public static List<Type> GetLoadedTypesAssignableFrom<T>()
        {
            var types = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                types.AddRange(assembly.GetTypes().Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract));
            }

            return types;
        }

        public static List<Type> GetLoadedTypesAssignableFrom(Type targetType)
        {
            var types = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                types.AddRange(assembly.GetTypes().Where(type => targetType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract));
            }

            return types;
        }
    }
}
