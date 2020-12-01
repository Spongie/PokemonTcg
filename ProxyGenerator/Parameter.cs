using System;
using System.Linq;

namespace ProxyGenerator
{
    internal class Parameter
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public override string ToString()
        {
            if (Type.IsGenericType)
            {
                string generics = $"<{string.Join(',', Type.GenericTypeArguments.Select(t => t.FullName))}>";
                return $"System.Collections.Generic.List{generics} {Name}";
            }
            if (Type.FullName.ToLower() == "system.void")
            {
                return $"void {Name}";
            }

            return $"{Type.FullName} {Name}";
        }
    }
}