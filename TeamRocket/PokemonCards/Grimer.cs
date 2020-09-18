using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Grimer : PokemonCard
    {
        public Grimer(Player owner) : base(owner)
        {
            PokemonName = "Grimer";
			Stage = 0;
            Hp = 40;
            Set = Singleton.Get<Set>();
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 1;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new PoisonGasGrimer(),
				new StickyHands()
            };
			
        }
    }
}
