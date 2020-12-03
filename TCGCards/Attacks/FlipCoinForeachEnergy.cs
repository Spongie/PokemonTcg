using CardEditor.Views;
using Entities;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class FlipCoinForeachEnergy : Attack
    {
        private bool discardForEachHead;

        [DynamicInput("Discard for each heads?", InputControl.Boolean)]
        public bool DiscardForeachHead
        {
            get { return discardForEachHead; }
            set
            {
                discardForEachHead = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var heads = game.FlipCoins(owner.ActivePokemonCard.AttachedEnergy.Count);

            if (heads > 0)
            {
                var message = new PickFromListMessage(owner.ActivePokemonCard.AttachedEnergy.OfType<Card>().ToList(), heads).ToNetworkMessage(owner.Id);
                var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);
                opponent.ActivePokemonCard.DiscardEnergyCard((EnergyCard)game.FindCardById(response.Cards.First()), game);
            }

            return heads * Damage;
        }
    }
}
