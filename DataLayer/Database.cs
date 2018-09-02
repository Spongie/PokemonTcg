using DataLayer.Queries;
using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace DataLayer
{
    public class Database : IDisposable
    {
        private static Database instance;
        private const string connectionString = @"Data Source=ENTERCYBER-PC\SQLEXPRESS;Initial Catalog=Pokemon_Tcg_Test;Integrated Security=True";
        public const string DateFormat = "yyyy-MM-dd HH:mm:ss";
        private SqlConnection connection;

        private Database()
        {

        }

        internal SqlCommand CreateCommand(string commandText) => CreateCommand(commandText, null);

        internal SqlCommand CreateCommand(string commandText, SqlTransaction transaction)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            return command;
        }

        internal SqlTransaction CreateTransaction() => connection.BeginTransaction();

        public void Connect()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
            Logger.Instance.Log("Connection to database established");
        }

        public IEnumerable<T> Select<T>() where T : DBEntity
        {
            return Select<T>(new SelectQuery<T>());
        }

        public IEnumerable<T> Select<T>(SelectQuery<T> query) where T : DBEntity
        {
            var result = new List<T>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = query.GenerateSql();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T resultObject = Activator.CreateInstance<T>();

                        for (int rowIndex = 0; rowIndex < reader.FieldCount; rowIndex++)
                        {
                            PropertyInfo property = typeof(T).GetProperty(reader.GetName(rowIndex), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            
                            if (property.PropertyType != typeof(DateTime))
                            {
                                property.SetValue(resultObject, reader.GetValue(rowIndex));
                            }
                            else if (property.PropertyType == typeof(bool))
                            {
                                bool value = reader.GetValue(rowIndex).ToString() == "1";
                                property.SetValue(resultObject, value);
                            }
                            else
                            {
                                var value = DateTime.ParseExact(reader.GetValue(rowIndex).ToString(), DateFormat, CultureInfo.InvariantCulture);
                                property.SetValue(resultObject, value);
                            }
                        }

                        result.Add(resultObject);
                    }
                }
            }

            return result;
        }

        public void Delete<T>(T entity) where T : DBEntity
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = new DeleteQuery<T>(entity).GenerateSql();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteList<T>(IEnumerable<T> entitys) where T : DBEntity
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = new DeleteMultipleQuery<T>(entitys).GenerateSql();
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteNonQuery(string query)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
        }

        public HashSet<string> SelectTables()
        {
            var tables = new HashSet<string>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }

            return tables;
        }

        public HashSet<string> SelectColumnsForTable(string table)
        {
            var columns = new HashSet<string>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}'";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(0));
                    }
                }
            }

            return columns;
        }

        public void Insert<T>(T objectToInsert) where T : DBEntity
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = new InsertQuery<T>(objectToInsert).GenerateSql();
                command.ExecuteNonQuery();
            }
        }

        public void InsertList<T>(IEnumerable<T> objects) where T : DBEntity
        {
            using (var command = connection.CreateCommand())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    command.Transaction = transaction;
                    foreach (var item in objects)
                    {
                        command.CommandText = new InsertQuery<T>(item).GenerateSql();
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        public void Update<T>(T objectToUpdate) where T : DBEntity
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = new UpdateQuery<T>(objectToUpdate).GenerateSql();
                command.ExecuteNonQuery();
            }
        }

        public static Database Instance
        {
            get
            {
                if (instance == null)
                    instance = new Database();

                return instance;
            }
        }

        public void CheckAndUpdate()
        {
            Logger.Instance.Log("Checking for database updates");

            HashSet<string> tables = SelectTables();

            foreach (var entityType in TypeLoader.GetLoadedTypesAssignableFrom<DBEntity>())
            {
                var entity = (DBEntity)Activator.CreateInstance(entityType);

                if (!tables.Contains(entity.GetTableName()))
                {
                    using (var command = connection.CreateCommand())
                    {
                        Logger.Instance.Log("Creating table for: " + entityType.FullName);
                        command.CommandText = TableCreator.GenerateCreateTableCommand(entityType);
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    CreateAndDropColumn(entityType, entity);
                }
            }

            foreach (var migrationType in TypeLoader.GetLoadedTypesAssignableFrom<IMigration>())
            {
                var migration = (IMigration)Activator.CreateInstance(migrationType);
                RunMigration(migration);
            }
            
            Logger.Instance.Log("Database updated");
        }

        private void CreateAndDropColumn(Type entityType, DBEntity entity)
        {
            HashSet<string> existingColumns = SelectColumnsForTable(entity.GetTableName());
            PropertyInfo[] properties = entityType.GetProperties();

            AddNewColumns(entityType, entity, existingColumns, properties);
            DropOldColumns(entityType, entity, existingColumns, properties);
        }

        private void DropOldColumns(Type entityType, DBEntity entity, HashSet<string> existingColumns, PropertyInfo[] properties)
        {
            foreach (var column in existingColumns)
            {
                if (!properties.Any(p => p.Name == column))
                {
                    Logger.Instance.Log($"Deleting old column {column} for: " + entityType.FullName);

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = TableCreator.GenerateDropColumnSql(entity.GetTableName(), column);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void AddNewColumns(Type entityType, DBEntity entity, HashSet<string> existingColumns, PropertyInfo[] properties)
        {
            foreach (var property in properties)
            {
                if (existingColumns.Contains(property.Name))
                {
                    continue;
                }

                Logger.Instance.Log($"Creating column {property} for: " + entityType.FullName);

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = TableCreator.GenerateAddColumnSql(entity.GetTableName(), property);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void RunMigration(IMigration migration)
        {
            var start = DateTime.Now;
            migration.Execute();
            var end = DateTime.Now;

            Insert(new Migration
            {
                Name = migration.GetType().FullName,
                StartTime = start,
                EndTime = end,
                ExecutionTimeId = migration.GetExecutionCreationTime()
            });

            Logger.Instance.Log($"Migration {migration.GetType().FullName} completed in {(int)(end - start).TotalMilliseconds}ms");
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
            Logger.Instance.Log("Connection to database closed");
        }
    }
}
