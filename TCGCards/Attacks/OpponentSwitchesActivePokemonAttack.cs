using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class OpponentSwitchesActivePokemonAttack : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!opponent.BenchedPokemon.Any())
                return;

            var response = opponent.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourBenchMessage(1).ToNetworkMessage(opponent.Id));
            var newActive = (PokemonCard)game.FindCardById(response.Cards.First());
            opponent.ForceRetreatActivePokemon(newActive, game);

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
