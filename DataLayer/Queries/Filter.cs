using System.Reflection;

namespace DataLayer.Queries
{
    internal abstract class Filter
    {
        public PropertyInfo Column { get; set; }
        public string Value { get; set; }

        public abstract string GenerateSql();
    }
}
