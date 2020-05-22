using System;
using System.Collections.Generic;

namespace TCGCards.Core
{
    public class GameLog
    {
        public List<string> Messages { get; set; } = new List<string>();
        public List<string> NewMessages { get; set; } = new List<string>();

        public void AddMessage(string message)
        {
            NewMessages.Add($"{DateTime.Now.Hour}:{DateTime.Now.Minute}: {message}");
        }

        public void CommitMessages()
        {
            Messages.AddRange(NewMessages);
            NewMessages.Clear();
        }
    }
}
