using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkCharmeleon : PokemonCard
    {
        public DarkCharmeleon(Player owner) : base(owner)
        {
            PokemonName = "Dark Charmeleon";
            Set = Singleton.Get<Set>();
            EvolvesFrom = "Charmander";
            Stage = 1;
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
