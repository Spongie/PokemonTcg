using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkAlakazam : IPokemonCard
    {
        public DarkAlakazam(Player owner) : base(owner)
        {
            PokemonName = "Dark Alakazam";
			EvolvesFrom = "Dark Kadabra"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 3;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new TeleportBlast(),
				new MindShock()
            };
			
        }
    }
}
