using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Abra : PokemonCard
    {
        public Abra(Player owner) : base(owner)
        {
            Hp = 40;
            Set = Singleton.Get<Set>();
            PokemonType = EnergyTypes.Psychic;
            Weakness = EnergyTypes.Psychic;
            Resistance = EnergyTypes.None;
            Stage = 0;
            RetreatCost = 1;
            Attacks = new List<Attack>
            {
                new Vanish(),
                new Psyshock()
            };

            PokemonName = PokemonNames.Abra;
        }
    }
}
