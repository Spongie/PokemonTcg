using DataLayer;
using System;

namespace Entities
{
    public class User : DBEntity
    {
        [DbLength(100)]
        public string UserName { get; set; }

        public string Password { get; set; }

        [DbLength(500)]
        public string Email { get; set; } = "";

        public DateTime RegisteredDate { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
