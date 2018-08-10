using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Abilities;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Rattata : IPokemonCard
    {
        protected Rattata(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.Rattata;
            Hp = 40;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 0;
            Stage = 0;
            Weakness = EnergyTypes.Fighting;
            Resistance = EnergyTypes.Psychic;
            Ability = new Trickery(this);
            Attacks = new List<Attack>
            {
                new QuickAttack()
            };
        }
    }
}
