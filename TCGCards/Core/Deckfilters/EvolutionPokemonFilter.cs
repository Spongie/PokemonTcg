namespace TCGCards.Core.Deckfilters
{
    public class EvolutionPokemonFilter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            return card is PokemonCard && ((PokemonCard)card).Stage > 0;
        }
    }
}
