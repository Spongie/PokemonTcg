using Server.Entities;
using System;

namespace Server.DataLayer.Migrations
{
    internal class InitialDatabaseMigration : IMigration
    {
        public void Execute()
        {
            using (var command = Database.Instance.CreateCommand(TableCreator.GenerateCreateTableCommand<Migration>()))
            {
                command.ExecuteNonQuery();
            }
        }

        public DateTime GetExecutionCreationTime() => new DateTime(2018, 8, 12, 15, 0, 0);
    }
}
