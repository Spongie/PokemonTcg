using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Metapod : PokemonCard
    {
        public Metapod(Player owner) : base(owner)
        {
            PokemonName = "Metapod";
            Set = Singleton.Get<Set>();
            EvolvesFrom = PokemonNames.Caterpie;
            Hp = 70;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Stiffen(),
				new StunSpore()
            };
			
        }
    }
}
