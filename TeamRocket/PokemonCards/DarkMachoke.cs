using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkMachoke : IPokemonCard
    {
        public DarkMachoke(Player owner) : base(owner)
        {
            PokemonName = "Dark Machoke";
			EvolvesFrom = "Machop"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 2;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new DragOff(),
				new KnockBack()
            };
			
        }
    }
}
