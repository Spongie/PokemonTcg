namespace TCGCards.Core.Deckfilters
{
    public class PokemonFilter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            return card is PokemonCard;
        }
    }
}
