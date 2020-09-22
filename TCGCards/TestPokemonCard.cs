using Entities;
using TCGCards.Core;

namespace TCGCards
{
    public class TestPokemonCard : PokemonCard
    {
        public TestPokemonCard(Player owner) : base(owner)
        {
            PokemonName = "Ekans";
            IsTestCard = true;
            Stage = 0;
            Hp = 50;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
            Resistance = EnergyTypes.None;
        }
    }
}
