using DataLayer;

namespace Entities
{
    public class DeckCard : DBEntity
    {
        public long DeckId { get; set; }
        public long CardId { get; set; }
        public int Quantity { get; set; }
    }
}
