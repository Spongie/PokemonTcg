using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Server.DataLayer
{
    public static class Util
    {
        private static Dictionary<SqlDbType, Type> sqlTypeMap = new Dictionary<SqlDbType, Type>
        {
            { SqlDbType.VarChar, typeof(string) },
            { SqlDbType.BigInt, typeof(long) },
            { SqlDbType.Int, typeof(int) },
            { SqlDbType.DateTime2, typeof(DateTime) },
            { SqlDbType.TinyInt, typeof(bool) }
        };

        private static Dictionary<SqlDbType, string> sqlTypeText = new Dictionary<SqlDbType, string>
        {
            { SqlDbType.VarChar, "VARCHAR({0})" },
            { SqlDbType.BigInt, "BIGINT" },
            { SqlDbType.Int, "INT" },
            { SqlDbType.DateTime2, "DATETIME2(3)" },
            { SqlDbType.TinyInt, "TINYINT" }
        };

        public static SqlDbType GetSqlTypeFromType(Type type) => sqlTypeMap.First(x => x.Value == type).Key;

        public static string GetSqlTypeString(SqlDbType type) => sqlTypeText[type];

        public static string GetValueSqlFormatted(object value, Type valueType)
        {
            var sqlType = GetSqlTypeFromType(valueType);

            switch (sqlType)
            {
                case SqlDbType.Float:
                case SqlDbType.Int:
                case SqlDbType.BigInt:
                    return value.ToString();
                case SqlDbType.DateTime2:
                    return $"'{((DateTime)value).ToString(Database.DateFormat)}'";
                case SqlDbType.VarChar:
                    return $"'{value.ToString()}'";
                case SqlDbType.TinyInt:
                    return ((bool)value) ? "1" : "0";
                default:
                    throw new NotImplementedException(sqlType + "not supported");
            }
        }
    }
}
