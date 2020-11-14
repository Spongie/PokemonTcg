using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class DestinyBond : Attack
    {
        public DestinyBond() :base()
        {
            Name = "Destiny Bond";
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

            protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
            {
                game.GameLog?.AddMessage($"{PokemonOwner.KnockedOutBy.GetName()} knocked out {PokemonOwner.Name} and dies itself");
                PokemonOwner.KnockedOutBy.DamageCounters = 9000;
            }
        }
    }
}
