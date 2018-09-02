using DataLayer;

namespace Entities
{
    public class CardInfo : DBEntity
    {
        [DbLength(500)]
        public string ClassName { get; set; }

        public long SetId { get; set; }
    }
}
