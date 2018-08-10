namespace TCGCards.Core.Deckfilters
{
    public class AnyCardFilter : IDeckFilter
    {
        public bool IsCardValid(ICard card)
        {
            return true;
        }
    }
}
