using System;
using System.Collections.Generic;
using System.Text;

namespace TCGCards.Core.Abilities
{
    public class ChaosGymAbility : Ability
    {
        public ChaosGymAbility() :this(null)
        {

        }

        public ChaosGymAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.TrainerCardPlayed;
        }

        public override bool CanActivate(GameField game, Player caster, Player opponent)
        {
            if (game.CurrentTrainerCard == null || game.CurrentTrainerCard.IsStadium())
            {
                return false;
            }

            return base.CanActivate(game, caster, opponent);
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            if (game.FlipCoins(1) == 1)
            {
                return;
            }

            if (!game.CurrentTrainerCard.CanCast(game, opponent, owner) || !game.AskYesNo(opponent, "Would you like to play the card instead?"))
            {
                return;
            }

            game.PlayTrainerCard(game.CurrentTrainerCard, opponent);
        }
    }
}
