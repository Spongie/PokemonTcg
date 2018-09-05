using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkKadabra : PokemonCard
    {
        public DarkKadabra(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkKadabra;
			EvolvesFrom = PokemonNames.Abra;
            Stage = 1;
            Hp = 50;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 2;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new MindShock()
            };
			//TODO: Pokemon power
        }
    }
}
