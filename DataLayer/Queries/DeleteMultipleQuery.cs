using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Queries
{
    public class DeleteMultipleQuery<T> where T : DBEntity
    {
        private readonly IEnumerable<T> entitys;

        public DeleteMultipleQuery(IEnumerable<T> entitys)
        {
            this.entitys = entitys;
        }

        public bool IsValid() => entitys.Any();

        public string GenerateSql()
        {
            Type type = typeof(T);
            string tableName = entitys.First().GetTableName();
            string where = $"WHERE ID IN ({string.Join(",", entitys.Select(x => x.Id))})";

            return $"DELETE FROM {tableName} {where}";
        }
    }
}
