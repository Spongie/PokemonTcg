using System;
using System.Collections.Generic;

namespace DataLayer.Queries
{
    public class InsertQuery<T> where T : DBEntity
    {
        private readonly Type targetType;
        private readonly T targetObject;

        public InsertQuery(T targetObject)
        {
            targetType = typeof(T);
            this.targetObject = targetObject;
        }

        public string GenerateSql()
        {
            var columns = new List<string>();
            var values = new List<string>();

            foreach (var property in targetType.GetProperties())
            {
                if (property.Name.ToLower() == "id")
                {
                    continue;
                }

                columns.Add(property.Name);
                var value = Util.GetValueSqlFormatted(property.GetValue(targetObject), property.PropertyType);
                values.Add(value);
            }

            return $"INSERT INTO {targetObject.GetTableName()} ({string.Join(",", columns)}) VALUES ({string.Join(",", values)})";
        }
    }
}
