using Server.DataLayer;
using System;

namespace Server.Entities
{
    public class LoginStatistics : DBEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime Time { get; set; }
        public bool Successful { get; set; }
    }
}
