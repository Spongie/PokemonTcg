using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Stare : Attack
    {
        public Stare()
        {
            Name = "Stare";
            Description = "Choose 1 of your opponent's Pokémon. This attack does 10 damage to that Pokémon. Don't apply Weakness and Resistance for this attack. (Any other effects that would happen after applying Weakness and Resistance still happen.) " +
                "If that Pokémon has a Pokémon Power, that power stops working until the end of your opponent's next turn.";
            DamageText = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var target = GameUtils.SelectOnePokemonCardFromOpponent(game, owner);

            if (target != null)
            {
                target.DamageCounters += 10;
                target.AbilityDisabled = true;
            }
        }
    }
}
