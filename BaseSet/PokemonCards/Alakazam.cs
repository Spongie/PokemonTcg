using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Alakazam : PokemonCard
    {
        public Alakazam(Player owner) : base(owner)
        {
            PokemonName = "Alakazam";
            EvolvesFrom = PokemonNames.Kadabra;
            Hp = 80;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 3;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new ConfuseRay()
            };
			//TODO: Pokemon power
        }
    }
}
