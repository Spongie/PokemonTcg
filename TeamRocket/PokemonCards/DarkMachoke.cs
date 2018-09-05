using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkMachoke : PokemonCard
    {
        public DarkMachoke(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkMachoke;
			EvolvesFrom = PokemonNames.Machop;
            Stage = 1;
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
