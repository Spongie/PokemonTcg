using System;
using System.Collections.Generic;

namespace TCGCards
{
    public static class Singleton
    {
        private static Dictionary<Type, object> instances = new Dictionary<Type, object>();
        private static object instanceLock = new object();

        public static T Get<T>()
        {
            var type = typeof(T);

            lock (instanceLock)
            {
                if (instances.ContainsKey(type))
                {
                    return (T)instances[type];
                }

                instances.Add(type, Activator.CreateInstance<T>());

                return (T)instances[type];
            }
        }
    }
}
