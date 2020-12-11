namespace TCGCards.Core.Deckfilters
{
    public class Stage2Filter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            var pokemon = card as PokemonCard;

            return pokemon != null && pokemon.Stage == 2;
        }
    }
}
