using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Abilities
{
    public class MatterExchange : Ability
    {
        public MatterExchange(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
            Name = "Matter Exchange";
            Description = "Discard a card, then draw a card";
        }

        public override bool CanActivate()
        {
            return PokemonOwner.Owner.Hand.Any() && base.CanActivate();
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new DiscardCardsMessage(1).ToNetworkMessage(owner.Id));
            owner.DiscardCards(response.Cards);
            owner.DrawCards(1);
        }
    }
}
