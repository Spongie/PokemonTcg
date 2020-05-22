using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class MirrorShell : Attack
    {
        public MirrorShell()
        {
            Name = "Mirror Shell";
            Description = "If an attack does damage to Dark Wartortle during your opponent's next turn (even if Dark Wartortle is Knocked Out), Dark Wartortle attacks the Defending Pokémon for an equal amount of damage.";
            DamageText = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.ActivePokemonCard.TemporaryAbilities.Add(new MirrorShellAbility(owner.ActivePokemonCard));
        }

        private class MirrorShellAbility : TemporaryAbility
        {
            public MirrorShellAbility(PokemonCard owner) :base(owner)
            {
                TriggerType = TriggerType.TakesDamage;
            }

            protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
            {
                opponent.ActivePokemonCard.DamageCounters += damageTaken;
            }
        }
    }
}
