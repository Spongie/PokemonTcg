using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.Messages;

namespace TeamRocket.Abilities
{
    public class ReelIn : Ability
    {
        public ReelIn(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.EnterPlay;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            var message = new SelectFromDiscard(3, new BasicPokemonFilter()).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            owner.Hand.AddRange(response.Cards);
            foreach (var card in response.Cards)
            {
                owner.DiscardPile.Remove(card);
            }
        }
    }
}
