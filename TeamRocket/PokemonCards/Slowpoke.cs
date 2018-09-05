using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Slowpoke : PokemonCard
    {
        public Slowpoke(Player owner) : base(owner)
        {
            Hp = 50;
            PokemonType = EnergyTypes.Psychic;
            Weakness = EnergyTypes.Psychic;
            Stage = 0;
            RetreatCost = 1;
            PokemonName = PokemonNames.Slowpoke;
            Attacks = new List<Attack>
            {
                new AfternoonNap(),
                new Headbutt()
            };
        }
    }
}
