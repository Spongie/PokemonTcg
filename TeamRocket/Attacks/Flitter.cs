using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Flitter : Attack
    {
        public Flitter()
        {
            Name = "Flitter";
            Description = "Choose 1 of your opponent's Pokémon. This attack does 20 damage to that Pokémon. Don't apply Weakness and Resistance for this attack. (Any other effects that would happen after applying Weakness and Resistance still happen.)";
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
            if (!opponent.BenchedPokemon.Any())
            {
                opponent.ActivePokemonCard.DamageCounters += 20;
                return;
            }

            var target = GameUtils.SelectOnePokemonCardFromOpponent(game, owner);

            if (target != null)
            {
                target.DamageCounters += 20;
            }
        }
    }
}
