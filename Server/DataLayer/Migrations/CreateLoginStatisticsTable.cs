using Server.Entities;
using System;

namespace Server.DataLayer.Migrations
{
    public class CreateLoginStatisticsTable : IMigration
    {
        public void Execute()
        {
            using (var command = Database.Instance.CreateCommand(TableCreator.GenerateCreateTableCommand<LoginStatistics>()))
            {
                command.ExecuteNonQuery();
            }
        }

        public DateTime GetExecutionCreationTime() => new DateTime(2018, 8, 12, 22, 4, 0);
    }
}
