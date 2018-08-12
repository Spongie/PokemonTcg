using Server.Entities;
using System;

namespace Server.DataLayer.Migrations
{
    public class CreateUserTable : IMigration
    {
        public void Execute()
        {
            using (var command = Database.Instance.CreateCommand(TableCreator.GenerateCreateTableCommand<User>()))
            {
                command.ExecuteNonQuery();
            }
        }

        public DateTime GetExecutionCreationTime() => new DateTime(2018, 8, 12, 19, 52, 0);
    }
}
