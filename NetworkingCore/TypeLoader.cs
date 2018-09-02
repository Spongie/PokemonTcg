using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkingCore
{
    public static class TypeLoader
    {
        public static IEnumerable<Type> GetLoadedTypesAssignableFrom<T>()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                return assembly.GetTypes().Where(type => typeof(T).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);
            }

            return new Type[] { };
        }
    }
}
