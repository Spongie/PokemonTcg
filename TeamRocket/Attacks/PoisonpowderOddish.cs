using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class PoisonpowderOddish : Attack
    {
        public PoisonpowderOddish()
        {
            Name = "Poisonpowder";
            Description = "The Defending Pokémon is now Poisoned.";
            DamageText = "";
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
            opponent.ActivePokemonCard.IsPoisoned = true;
        }
    }
}
