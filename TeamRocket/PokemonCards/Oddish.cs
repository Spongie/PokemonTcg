using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Oddish : IPokemonCard
    {
        public Oddish(Player owner) : base(owner)
        {
            PokemonName = "Oddish";
			Stage = 0;
            Hp = 50;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new SleepPowder(),
				new Poisonpowder()
            };
			
        }
    }
}
