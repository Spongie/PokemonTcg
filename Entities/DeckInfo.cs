using DataLayer;
using System.Collections.Generic;

namespace Entities
{
    public class DeckInfo : DBEntity
    {
        [DbLength(250)]
        public string Name { get; set; }

        [DbIgnore]
        public List<DeckCard> Cards { get; set; }

        public long UserId { get; set; }
    }
}
