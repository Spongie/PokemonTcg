using DataLayer;
using System.Collections.Generic;

namespace Entities
{
    public class CardSet : DBEntity
    {
        [DbLength(100)]
        public string Name { get; set; }

        public long SetId { get; set; }

        [DbIgnore]
        public List<CardInfo> Cards { get; set; }

        public static CardSet CreateFrom(ISet set)
        {
            return new CardSet
            {
                SetId = set.GetId(),
                Name = set.GetName()
            };
        }
    }
}
