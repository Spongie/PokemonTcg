using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Queries
{
    internal class InFilter : Filter
    {
        public List<string> Values { get; set; }

        public override string GenerateSql()
        {
            return $"{Column.Name} IN ({string.Join(",", Values.Select(x => Util.GetValueSqlFormatted(x, Column.PropertyType)))})";
        }
    }
}