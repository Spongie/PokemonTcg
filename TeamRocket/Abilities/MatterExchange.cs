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
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new DiscardCardsMessage(1).ToNetworkMessage(owner.Id));
            owner.DiscardCards(response.Cards);
            owner.DrawCards(1);
        }
    }
}
