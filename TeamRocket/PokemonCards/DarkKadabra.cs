using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkKadabra : IPokemonCard
    {
        public DarkKadabra(Player owner) : base(owner)
        {
            PokemonName = "Dark Kadabra";
			EvolvesFrom = "Abra"; //TODO: Add stage
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
