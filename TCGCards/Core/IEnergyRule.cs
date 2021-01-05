namespace TCGCards.Core
{
    public interface IEnergyRule
    {
        bool CanPlayEnergyCard(EnergyCard card);
        void CardPlayed();
        void Reset();
    }
}
