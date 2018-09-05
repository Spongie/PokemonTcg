using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkGyarados : PokemonCard
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
            Attacks = new List<Attack> { new IceBeam() };
        }

        public override string GetName()
        {
            return "Dark Gyarados";
        }
    }
}
