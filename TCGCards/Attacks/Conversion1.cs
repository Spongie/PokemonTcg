using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class Conversion1 : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var message = new SelectColorMessage("Select a new weakness for your opponents active pokemon").ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<SelectColorMessage>(message);

            opponent.ActivePokemonCard.Weakness = response.Color;

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
