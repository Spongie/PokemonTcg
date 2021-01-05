namespace TCGCards.Core
{
    public struct StandardEnergyRule : IEnergyRule
    {
        public bool CanPlayEnergyCard(EnergyCard card)
        {
            return CardsPlayedThisTurn == 0;
        }

        public void Reset()
        {
            CardsPlayedThisTurn = 0;
        }

        public void CardPlayed()
        {
            CardsPlayedThisTurn++;
        }

        public int CardsPlayedThisTurn { get; set; }
    }
}
