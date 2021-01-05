namespace TCGCards.Core
{
    public struct StandardEnergyRule : IEnergyRule
    {
        public bool CanPlayEnergyCard(EnergyCard card, PokemonCard target)
        {
            return CardsPlayedThisTurn == 0;
        }

        public void Reset()
        {
            CardsPlayedThisTurn = 0;
        }

        public void CardPlayed(EnergyCard card, PokemonCard target)
        {
            CardsPlayedThisTurn++;
        }

        public int CardsPlayedThisTurn { get; set; }
    }
}
