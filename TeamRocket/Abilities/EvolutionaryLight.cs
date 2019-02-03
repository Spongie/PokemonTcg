using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Abilities
{
    public class EvolutionaryLight : Ability
    {
        public EvolutionaryLight(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
            Name = "Evolutionary Light";
            Description = "Search your deck for an evolution card and put it into your hand";
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            var message = new DeckSearchMessage(owner, new List<IDeckFilter> { new Filter() }, 1).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            owner.DrawCardsFromDeck(response.Cards);
        }

        private class Filter : IDeckFilter
        {
            public bool IsCardValid(Card card)
            {
                return card is PokemonCard && ((PokemonCard)card).Stage > 0;
            }
        }
    }
}
