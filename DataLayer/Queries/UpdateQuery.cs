﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataLayer.Queries
{
    public class UpdateQuery<T> where T : DBEntity
    {
        private readonly T entity;

        public UpdateQuery(T entity)
        {
            this.entity = entity;
        }

        public string GenerateSql()
        {
            var updates = new List<UpdateField>();
            var filters = "";

            foreach (var property in typeof(T).GetProperties())
            {
                if (property.GetCustomAttribute<DbIgnore>() != null)
                {
                    continue;
                }
                if (property.Name.ToLower() == "id")
                {
                    filters = "WHERE ID = " + Util.GetValueSqlFormatted(property.GetValue(entity), property.PropertyType);
                    continue;
                }
                
                var value = Util.GetValueSqlFormatted(property.GetValue(entity), property.PropertyType);
                updates.Add(new UpdateField(property.Name, value));
            }

            return $"UPDATE {entity.GetTableName()} SET {string.Join(",", updates.Select(x => x.GenerateQuery()))} {filters}";
        }
    }
}
