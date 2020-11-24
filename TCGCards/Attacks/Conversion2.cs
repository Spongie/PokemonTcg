using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class Conversion2 : Attack
    {
        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var message = new SelectColorMessage("Change Porygon's Resistance to a type of your choice other than Colorless.").ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<SelectColorMessage>(message);

            owner.ActivePokemonCard.Resistance = response.Color;

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
