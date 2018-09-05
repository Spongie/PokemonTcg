using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkFlareon : PokemonCard
    {
        public DarkFlareon(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkFlareon;
			EvolvesFrom = PokemonNames.Eevee;
            Stage = 1;
            Hp = 50;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 1;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Rage(),
				new PlayingwithFire()
            };
			
        }
    }
}
