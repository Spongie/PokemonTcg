using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Weedle : PokemonCard
    {
        public Weedle(Player owner) : base(owner)
        {
            PokemonName = "Weedle";
            Set = Singleton.Get<Set>();
            Hp = 40;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new PoisonSting()
            };
			
        }
    }
}
