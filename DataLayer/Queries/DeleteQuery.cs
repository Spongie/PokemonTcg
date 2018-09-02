using System;

namespace DataLayer.Queries
{
    public class DeleteQuery<T> where T : DBEntity
    {
        private readonly T entity;

        public DeleteQuery(T entity)
        {
            this.entity = entity;
        }

        public string GenerateSql()
        {
            Type type = typeof(T);
            string tableName = entity.GetTableName();
            string where = $"WHERE ID = {entity.Id}";

            return $"DELETE FROM {tableName} {where}";
        }
    }
}
