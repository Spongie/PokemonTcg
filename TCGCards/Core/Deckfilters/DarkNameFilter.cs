namespace TCGCards.Core.Deckfilters
{
    public class DarkNameFilter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            return card.GetName().ToLower().Contains("dark");
        }
    }
}
