using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Magikarp : PokemonCard
    {
        public Magikarp() :this(null)
        {

        }

        public Magikarp(Player owner) :base(owner)
        {
            Hp = 30;
            PokemonType = EnergyTypes.Water;
            Weakness = EnergyTypes.Electric;
            Resistance = EnergyTypes.None;
            Stage = 0;
            RetreatCost = 1;
            Attacks = new List<Attack>
            {
                new Flop()
            };
            PokemonName = PokemonNames.Magikarp;
        }
    }
}
