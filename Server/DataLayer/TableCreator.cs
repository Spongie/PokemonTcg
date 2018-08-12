using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Server.DataLayer
{
    public static class TableCreator
    {     
        public static string GenerateCreateTableCommand<T>() where T : DBEntity
        {
            var tableName = typeof(T).FullName.Replace('.', '_');
            var columnCommands = new List<string>();

            foreach (var property in typeof(T).GetProperties())
            {
                string columnText = GenerateCreateColumnCommand(property);

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

        private static string GenerateCreateColumnCommand(PropertyInfo property)
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
