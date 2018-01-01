using TCGCards.Core;

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
            EvolvesFrom = new Magikarp();
        }

        public override string GetName()
        {
            return "Dark Gyarados";
        }
    }
}
