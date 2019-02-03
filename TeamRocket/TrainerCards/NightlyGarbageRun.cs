using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.TrainerCards
{
    public class NightlyGarbageRun : TrainerCard
    {
        public NightlyGarbageRun()
        {
            Name = "Nightly Garbage Run";
            Description = "Choose up to 3 basic Pokémon cards, Evolution cards, and/or basic Energy cards from your discard pile. Show them to your opponent and shuffle them into your deck";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromDiscard(3, new Filter()).ToNetworkMessage(caster.Id));

            caster.Deck.ShuffleInCards(response.Cards);

            foreach (var card in response.Cards)
            {
                caster.DiscardPile.Remove(card);
            }
        }

        private class Filter : IDeckFilter
        {
            public bool IsCardValid(Card card)
            {
                bool isPokemon = card is PokemonCard;
                bool isBasicEnergy = card is EnergyCard && ((EnergyCard)card).IsBasic;

                return isPokemon || isBasicEnergy;
            }
        }
    }
}
