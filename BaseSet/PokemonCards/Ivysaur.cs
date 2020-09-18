using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Ivysaur : PokemonCard
    {
        public Ivysaur(Player owner) : base(owner)
        {
            PokemonName = "Ivysaur";
            Set = Singleton.Get<Set>();
            EvolvesFrom = PokemonNames.Bulbasaur;
            Hp = 60;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new VineWhip(),
				new Poisonpowder()
            };
			
        }
    }
}
