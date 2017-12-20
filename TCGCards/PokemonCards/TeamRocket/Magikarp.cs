using Assets.Scripts.Cards.PokemonCards.TeamRocket.Attacks;
using System.Collections.Generic;

namespace Assets.Scripts.Cards.PokemonCards.TeamRocket
{
    public class Magikarp : IPokemonCard
    {
        public Magikarp()
        {
            Hp = 30;
            PokemonType = EnergyTypes.Water;
            Weakness = EnergyTypes.Electric;
            Resistance = EnergyTypes.None;
            Stage = 0;
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
