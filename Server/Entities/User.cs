using Server.DataLayer;
using System;

namespace Server.Entities
{
    public class User : DBEntity
    {
        [DbLength(100)]
        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime RegisteredDate { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
