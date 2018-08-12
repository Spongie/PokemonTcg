using Server.DataLayer;
using System;

namespace Server.Entities
{
    public class Migration : DBEntity
    {
        [DbLength(300)]
        public string Name { get; set; }
        public DateTime ExecutionTimeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
