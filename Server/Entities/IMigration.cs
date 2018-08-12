using System;

namespace Server.Entities
{
    public interface IMigration
    {
        void Execute();
        DateTime GetExecutionCreationTime();
    }
}
