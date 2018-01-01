using System;
using System.Collections.Generic;
using TCGCards.Core;
using TCGCards.PokemonCards.TeamRocket.Attacks;

namespace TCGCards.PokemonCards.TeamRocket
{
    public class Magikarp : IPokemonCard
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
        }

        public override string GetName()
        {
            return "Magikarp";
        }
    }
}
