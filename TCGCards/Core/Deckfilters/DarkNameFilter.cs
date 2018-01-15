using TCGCards.Core;

namespace TCGCards.Core.Deckfilters
{
    public class DarkNameFilter : IDeckFilter
    {
        public bool IsCardValid(ICard card)
        {
            return card.GetName().ToLower().Contains("dark");
        }
    }
}
