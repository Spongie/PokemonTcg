using TCGCards;
using TCGCards.Core;

namespace TeamRocket.TrainerCards
{
    public class GoopGasAttack : TrainerCard
    {
        public GoopGasAttack()
        {
            Name = "Goop Gas Attack";
            Description = "All Pokemon Powers stop working until the end of your opponents next turn";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            game.TemporaryPassiveAbilities.Add(new GoopGasAttackAbility(null));
        }

        private class GoopGasAttackAbility : TemporaryPassiveAbility
        {
            public GoopGasAttackAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
            {
                ModifierType = PassiveModifierType.NoPokemonPowers;
            }
        }
    }
}
