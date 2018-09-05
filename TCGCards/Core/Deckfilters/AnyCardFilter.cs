namespace TCGCards.Core.Deckfilters
{
    public class AnyCardFilter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            return true;
        }
    }
}
