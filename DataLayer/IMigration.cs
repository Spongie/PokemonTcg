using System;

namespace DataLayer
{
    public interface IMigration
    {
        void Execute();
        DateTime GetExecutionCreationTime();
    }
}
