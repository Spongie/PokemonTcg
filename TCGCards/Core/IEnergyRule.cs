namespace TCGCards.Core
{
    public interface IEnergyRule
    {
        bool CanPlayEnergyCard(EnergyCard card, PokemonCard target);
        void CardPlayed(EnergyCard card, PokemonCard target);
        void Reset();
    }
}
