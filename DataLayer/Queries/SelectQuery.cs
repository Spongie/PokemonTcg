using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataLayer.Queries
{
    public class SelectQuery<T> where T : DBEntity
    {
        private readonly Type targetType;
        private readonly List<string> overrideSelects;
        private readonly List<Filter> filters;
        private int limit = -1;

        public SelectQuery()
        {
            targetType = typeof(T);
            overrideSelects = new List<string>();
            filters = new List<Filter>();
        }

        public SelectQuery<T> Select(string name)
        {
            if (name.Contains(";"))
                throw new InvalidOperationException("name contained invalid characters");

            overrideSelects.Add(name);
            return this;
        }

        public SelectQuery<T> AndEquals(string column, string equals)
        {
            return AndEquals(targetType.GetProperty(column, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), equals);
        }

        public SelectQuery<T> Limit(int limit)
        {
            this.limit = limit;
            return this;
        }

        public SelectQuery<T> AndEquals(PropertyInfo column, string equals)
        {
            filters.Add(new AndFilter
            {
                Column = column,
                Value = equals
            });

            return this;
        }

        public SelectQuery<T> AndIn(string column, IEnumerable<string> values)
        {
            return AndIn(targetType.GetProperty(column, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), values);
        }

        public SelectQuery<T> AndIn(PropertyInfo column, IEnumerable<string> values)
        {
            filters.Add(new InFilter
            {
                Column = column,
                Values = values.ToList()
            });

            return this;
        }

        public string GenerateSql()
        {
            string tableName = targetType.FullName.Replace('.', '_');
            string selects = overrideSelects.Any() ? string.Join(",", overrideSelects) : "*";
            string filterQuery = "";

            if (filters.Any())
            {
                filterQuery = "WHERE ";
                bool first = true;

                foreach (var filter in filters)
                {
                    if (!first)
                    {
                        filterQuery += " AND ";
                    }

                    filterQuery += filter.GenerateSql();
                    first = false;
                }

            }

            string limitQuery = limit > 0 ? $"TOP {limit} " : "";

            return $"SELECT {limitQuery}{selects} FROM {tableName} {filterQuery}".Trim();
        }
    }
}
