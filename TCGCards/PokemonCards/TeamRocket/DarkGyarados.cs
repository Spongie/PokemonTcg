using TCGCards.Core;
using TCGCards.PokemonCards.TeamRocket.Abilities;

namespace TCGCards.PokemonCards.TeamRocket
{
    public class DarkGyarados : IPokemonCard
    {
        public DarkGyarados() : this(null)
        {

        }

        public DarkGyarados(Player owner) : base(owner)
        {
            Hp = 70;
            PokemonType = EnergyTypes.Water;
            Weakness = EnergyTypes.Grass;
            Resistance = EnergyTypes.Fighting;
            Stage = 1;
            RetreatCost = 2;
            EvolvesFrom = PokemonNames.Magikarp;
            PokemonName = PokemonNames.Gyarados;
            Ability = new FinalBeam(this);
        }

        public override string GetName()
        {
            return "Dark Gyarados";
        }
    }
}
