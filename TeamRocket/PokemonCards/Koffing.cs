using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Koffing : IPokemonCard
    {
        public Koffing(Player owner) : base(owner)
        {
            PokemonName = "Koffing";
			Stage = 0;
            Hp = 40;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Tackle(),
				new PoisonGas()
            };
			
        }
    }
}
