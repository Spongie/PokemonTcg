using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Nidoking : PokemonCard
    {
        public Nidoking(Player owner) : base(owner)
        {
            PokemonName = "Nidoking";
			EvolvesFrom = PokemonNames.Nidorino;
            Hp = 90;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 3;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new Thrash(),
				new Toxic()
            };
			
        }
    }
}
