using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class DestinyBond : Attack
    {
        public DestinyBond()
        {
            Name = "Destiny Bond";
            Description = "Discard 1 Energy card attached to Gastly in order to use this attack. If a Pokémon Knocks Out Gastly during your opponent's next turn, Knock Out that Pokémon.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override void PayExtraCosts(GameField game, Player owner, Player opponent)
        {
            AttackUtils.DiscardAttachedEnergy(owner.ActivePokemonCard, 1);
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.ActivePokemonCard.TemporaryAbilities.Add(new DestinyBondAbilty(owner.ActivePokemonCard));
        }

        private class DestinyBondAbilty : TemporaryAbility
        {
            public DestinyBondAbilty(PokemonCard owner) : base(owner)
            {
                TriggerType = TriggerType.Dies;
            }

            protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
            {
                log.AddMessage($"{PokemonOwner.KnockedOutBy.GetName()} knocked out ghastly and dies itself");
                PokemonOwner.KnockedOutBy.DamageCounters = 9000;
            }
        }
    }
}
