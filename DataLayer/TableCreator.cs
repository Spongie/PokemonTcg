using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DataLayer
{
    public static class TableCreator
    {     
        public static string GenerateCreateTableCommand<T>() where T : DBEntity
        {
            return GenerateCreateTableCommand(typeof(T));
        }

        public static string GenerateCreateTableCommand(Type type)
        {
            var tableName = type.FullName.Replace('.', '_');
            var columnCommands = new List<string>();

            foreach (var property in type.GetProperties())
            {
                string columnText = GenerateCreateTableColumnCommand(property);

                if (property.GetCustomAttribute<DbIgnore>() != null)
                {
                    continue;
                }

                if (columnText.StartsWith("Id"))
                {
                    columnCommands.Insert(0, columnText);
                }
                else
                {
                    columnCommands.Add(columnText);
                }
            }

            string createColumnCommand = string.Join(",", columnCommands);

            return $"CREATE TABLE {tableName} ({createColumnCommand});";
        }

        public static string GenerateAddColumnSql(string tableName, PropertyInfo property)
        {
            SqlDbType dbType = Util.GetSqlTypeFromType(property.PropertyType);
            string typeText = Util.GetSqlTypeString(dbType);

            if (typeText.Contains("{0}"))
            {
                var lengthAttribute = property.GetCustomAttribute<DbLengthAttribute>();
                typeText = typeText.Replace("{0}", lengthAttribute == null ? "1000" : lengthAttribute.Length.ToString());
            }

            return $"ALTER TABLE {tableName} ADD {property.Name} {typeText.Trim()}";
        }

        public static string GenerateDropColumnSql(string tableName, string column)
        {
            return $"ALTER TABLE {tableName} DROP COLUMN {column}";
        }

        private static string GenerateCreateTableColumnCommand(PropertyInfo property)
        {
            SqlDbType dbType = Util.GetSqlTypeFromType(property.PropertyType);
            string typeText = Util.GetSqlTypeString(dbType);
            string keyCommands = string.Empty;

            if (typeText.Contains("{0}"))
            {
                var lengthAttribute = property.GetCustomAttribute<DbLengthAttribute>();
                typeText = typeText.Replace("{0}", lengthAttribute == null ? "1000" : lengthAttribute.Length.ToString());
            }

            if (property.Name.ToLower() == "id")
            {
                keyCommands = " IDENTITY(1,1) PRIMARY KEY";
            }

            return $"{property.Name.Trim()} {typeText.Trim()}{keyCommands.TrimEnd()}";
        }
    }
}
