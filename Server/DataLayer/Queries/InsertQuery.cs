using System;
using System.Collections.Generic;

namespace Server.DataLayer.Queries
{
    public class InsertQuery<T> where T : DBEntity
    {
        private readonly Type targetType;
        private readonly object targetObject;

        public InsertQuery(object targetObject)
        {
            targetType = typeof(T);
            this.targetObject = targetObject;
        }

        public string GenerateSql()
        {
            string tableName = targetType.FullName.Replace('.', '_');
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

            return $"INSERT INTO {tableName} ({string.Join(',', columns)}) VALUES ({string.Join(',', values)})";
        }
    }
}
