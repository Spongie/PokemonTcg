namespace TCGCards.Core.Deckfilters
{
    public struct EvolvesFromPokemonFilter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            return card is PokemonCard && ((PokemonCard)card).EvolvesFrom == Name;
        }

        public string Name { get; set; }
    }
}
