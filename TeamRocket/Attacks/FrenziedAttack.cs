using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    public class FrenziedAttack : Attack
    {
        public FrenziedAttack()
        {
            Name = "Frenzied Attack";
            Description = "Dark Primeape is now Confused (after doing damage).";
            DamageText = "40";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 40;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.ActivePokemonCard.IsConfused = true;
        }
    }
}
