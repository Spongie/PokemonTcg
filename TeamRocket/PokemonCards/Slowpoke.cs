using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Slowpoke : IPokemonCard
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
                new Headbutt()
            };
            Ability = new AfternoonNap(this);
        }

        public override string GetName()
        {
            return PokemonNames.Slowpoke;
        }
    }
}
