using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkCharmeleon : IPokemonCard
    {
        public DarkCharmeleon(Player owner) : base(owner)
        {
            PokemonName = "Dark Charmeleon";
			EvolvesFrom = "Charmander";
            Stage = 0;
            Hp = 50;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 2;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new TailSlap(),
				new Fireball()
            };
			
        }
    }
}
