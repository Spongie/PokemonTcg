using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Ponyta : IPokemonCard
    {
        public Ponyta(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.Ponyta;
            Hp = 40;
            PokemonType = EnergyTypes.Fire;
            Weakness = EnergyTypes.Water;
            RetreatCost = 1;
            Stage = 0;
            Attacks = new List<Attack>
            {
                new Ember()
            };
        }
    }
}
