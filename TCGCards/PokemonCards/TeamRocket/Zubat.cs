using System.Collections.Generic;
using TCGCards.Core;
using TCGCards.PokemonCards.TeamRocket.Attacks;

namespace TCGCards.PokemonCards.TeamRocket
{
    public class Zubat : IPokemonCard
    {
        public Zubat(Player owner) : base(owner)
        {
            Hp = 40;
            PokemonType = EnergyTypes.Grass;
            Weakness = EnergyTypes.Psychic;
            Resistance = EnergyTypes.Fighting;
            Stage = 0;
            RetreatCost = 0;
            PokemonName = PokemonNames.Zubat;
            Attacks = new List<Attack>
            {
                new Ram(),
                new Bite()
            };
        }

        public override string GetName()
        {
            return "Zubat";
        }
    }
}
