using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Harden : Attack
    {
        public Harden()
        {
            Name = "Harden";
            Description = "During opponent's next turn, whenever 30 or less damage is done to Onix (after applying Weakness and Resistance), prevent that damage. (Any other effects of attacks still happen.)";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.ActivePokemonCard.TemporaryAbilities.Add(new HardenAbility(owner.ActivePokemonCard));
        }

        private class HardenAbility : TemporaryAbility
        {
            public HardenAbility(PokemonCard owner) : base(owner)
            {
                TriggerType = TriggerType.TakesDamage;
            }

            protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
            {
                if (damageTaken > 30)
                {
                    log.AddMessage("Damge taken was over 30 no effect from Harden");
                    return;
                }

                log.AddMessage("Damge taken was less than 30 damage prevented by Harden");
                owner.ActivePokemonCard.DamageCounters -= damageTaken;
            }
        }
    }
}
