using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.Attacks
{
    internal class Metronome : Attack
    {
        private Attack chosenAttack;

        public Metronome()
        {
            Name = "Metronome";
            Description = "Choose 1 of Defending Pokémon's attacks. Metronome copies that attack except for its Energy costs and anything else required in order to use that attack, such as discarding energy cards. (No matter what type the defender is, Clefairy's type is still Colorless.)";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var attacks = opponent.ActivePokemonCard.Attacks;

            if (attacks.Count == 1)
            {
                chosenAttack = attacks[0];
            }
            else
            {
                var attackMessage = new SelectAttackMessage(opponent.ActivePokemonCard.Attacks).ToNetworkMessage(owner.Id);
                chosenAttack = owner.NetworkPlayer.SendAndWaitForResponse<Attack>(attackMessage);
            }

            return chosenAttack.GetDamage(owner, opponent, game);
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            chosenAttack.ProcessEffects(game, owner, opponent);
            chosenAttack = null;
        }
    }
}
