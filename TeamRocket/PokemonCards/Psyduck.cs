using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class Psyduck : PokemonCard
    {
        protected Psyduck(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.Psyduck;
            Hp = 50;
            Set = Singleton.Get<Set>();
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Stage = 0;
            Weakness = EnergyTypes.Electric;
            Attacks = new List<Attack>
            {
                new Dizziness(),
                new WaterGun()
            };
        }
    }
}
