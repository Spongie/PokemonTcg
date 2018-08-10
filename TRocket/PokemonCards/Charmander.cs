using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Charmander : IPokemonCard
    {
        public Charmander(Player owner) : base(owner)
        {
            PokemonName = "Charmander";
			Stage = 0;
            Hp = 40;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 1;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new FireTail()
            };
			//TODO: Pokemon power
        }
    }
}
