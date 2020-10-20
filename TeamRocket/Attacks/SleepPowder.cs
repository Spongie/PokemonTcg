using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class SleepPowder : Attack
    {
        public SleepPowder()
        {
            Name = "Sleep Powder";
            Description = "The Defending Pokémon is now Asleep.";
            DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.ActivePokemonCard.IsAsleep = true;
        }
    }
}
