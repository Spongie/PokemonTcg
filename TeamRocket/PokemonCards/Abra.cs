using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.PokemonCards
{
    public class Abra : IPokemonCard
    {
        public Abra(Player owner) : base(owner)
        {
            Hp = 40;
            PokemonType = EnergyTypes.Psychic;
            Weakness = EnergyTypes.Psychic;
            Resistance = EnergyTypes.None;
            Stage = 0;
            RetreatCost = 1;
            Attacks = new List<Attack>
            {
                
            };

            PokemonName = PokemonNames.Abra;
        }

        public override string GetName()
        {
            return "Abra";
        }
    }
}
