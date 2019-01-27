using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class ThirdEye : Attack
    {
        public ThirdEye()
        {
            Name = "Third Eye";
            Description = "Discard 1 Energy card attached to Dark Golduck in order to draw up to 3 cards.";
            DamageText = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var message = new PickFromListMessage(owner.ActivePokemonCard.AttachedEnergy, new EnergyFilter(), 1).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            owner.ActivePokemonCard.DiscardEnergyCard(response.Cards.OfType<EnergyCard>().First());
            owner.DrawCards(3);
        }
    }
}
