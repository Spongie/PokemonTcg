using System;

namespace ProxyGenerator
{
    internal class Parameter
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public override string ToString()
        {
            return $"{Type.FullName} {Name}";
        }
    }
}