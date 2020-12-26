using NetworkingCore.Messages;
using System;
using System.Linq;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Core.Abilities
{
    public class Peek : Ability
    {
        public Peek() :this(null)
        {

        }

        public Peek(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            if (game.AskYesNo(owner, "Look at the top card of opponents deck?"))
            {
                owner.RevealCard(opponent.Deck.Cards.Peek());
            }
            else if (game.AskYesNo(owner, "Look at the top card of your deck?"))
            {
                owner.RevealCard(owner.Deck.Cards.Peek());
            }
            else if (game.AskYesNo(owner, "Look at a random card from opponents hand?"))
            {
                owner.RevealCard(opponent.Hand[new Random().Next(0, opponent.Hand.Count)]);
            }
            else if (game.AskYesNo(owner, "Look at a prize card from opponent?"))
            {
                var message = new PickFromListMessage(opponent.PrizeCards, 1).ToNetworkMessage(game.Id);
                var response = opponent.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.FirstOrDefault();

                owner.RevealCard(game.Cards[response]);
            }
            else if (game.AskYesNo(owner, "Look at one of your prize cards?"))
            {
                var message = new PickFromListMessage(owner.PrizeCards, 1).ToNetworkMessage(game.Id);
                var response = opponent.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.FirstOrDefault();

                owner.RevealCard(game.Cards[response]);
            }
        }
    }
}
