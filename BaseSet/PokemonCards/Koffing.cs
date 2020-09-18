using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Koffing : PokemonCard
    {
        public Koffing(Player owner) : base(owner)
        {
            PokemonName = "Koffing";
            Set = Singleton.Get<Set>();
            Hp = 50;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new FoulGas()
            };
			
        }
    }
}
