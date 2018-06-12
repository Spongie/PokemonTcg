using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Porygon : IPokemonCard
    {
        public Porygon(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.Porygon;
            Hp = 40;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 0;
            Weakness = EnergyTypes.Fighting;
            Resistance = EnergyTypes.Psychic;
            Attacks = new List<Attack>
            {
                new Conversion1(),
                new PsyBeam()
            };
        }

        public override string GetName()
        {
            return PokemonName;
        }
    }
}
