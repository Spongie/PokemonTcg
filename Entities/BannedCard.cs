using DataLayer;

namespace Entities
{
    public class BannedCard : DBEntity
    {
        public long FormatId { get; set; }
        public long CardId { get; set; }
    }
}
