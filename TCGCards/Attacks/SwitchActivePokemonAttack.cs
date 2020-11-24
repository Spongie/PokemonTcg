using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class SwitchActivePokemonAttack : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!opponent.BenchedPokemon.Any())
                return;

            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromOpponentBenchMessage(1).ToNetworkMessage(opponent.Id));
            var newActive = (PokemonCard)game.FindCardById(response.Cards.First());
            opponent.ForceRetreatActivePokemon(newActive, game);

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
