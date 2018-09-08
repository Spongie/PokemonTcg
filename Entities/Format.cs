using DataLayer;
using System.Collections.Generic;

namespace Entities
{
    public class Format : DBEntity
    {
        public string Name { get; set; }

        public long FormatId { get; set; }

        [DbIgnore]
        public List<FormatSet> Sets { get; set; }

        [DbIgnore]
        public List<BannedCard> BannedCards { get; set; }

        public static DBEntity CreateFrom(IFormat format)
        {
            return new Format
            {
                Name = format.GetName()
            };
        }
    }
}
