using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Toxic : Attack
    {
        public Toxic()
        {
            Name = "Toxic";
            Description = "The Defending Pokémon is now Poisoned. It now takes 20 Poison damage instead of 10 after each player's turn (even if it was already Poisoned).";
			DamageText = "40";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 40;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            opponent.ActivePokemonCard.IsPoisoned = true;
            opponent.ActivePokemonCard.DoublePoison = true;
        }
    }
}
