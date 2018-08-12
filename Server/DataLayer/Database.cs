using Server.DataLayer.Queries;
using Server.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Server.DataLayer
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

        public void Insert<T>(T objectToInsert) where T : DBEntity
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = new InsertQuery<T>(objectToInsert).GenerateSql();
                command.ExecuteNonQuery();
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

            HashSet<DateTime> executedMigrations = new HashSet<DateTime>();

            try
            {
                executedMigrations = Select<Migration>().Select(x => x.ExecutionTimeId).ToHashSet();
            }
            catch (SqlException sqlException)
            {
                if (sqlException.Message != "Invalid object name 'SERVER_ENTITIES_MIGRATION'.")
                {
                    throw;
                }
            }

            foreach (var migrationType in Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(IMigration).IsAssignableFrom(type) && type.IsClass))
            {
                var migration = (IMigration)Activator.CreateInstance(migrationType);

                if (executedMigrations.Contains(migration.GetExecutionCreationTime()))
                {
                    continue;
                }

                RunMigration(migration);
            }

            Logger.Instance.Log("Database updated");
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
