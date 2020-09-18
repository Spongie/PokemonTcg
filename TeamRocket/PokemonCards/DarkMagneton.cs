using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkMagneton : PokemonCard
    {
        public DarkMagneton(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkMagneton;
            EvolvesFrom = PokemonNames.Magnemite;
            Stage = 1;
            Hp = 60;
            Set = Singleton.Get<Set>();
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Sonicboom(),
				new MagneticLines()
            };
			
        }
    }
}
