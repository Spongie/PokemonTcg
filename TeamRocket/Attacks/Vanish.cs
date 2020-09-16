using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class Vanish : Attack
    {
        public Vanish()
        {
            Name = "Vanish";
            Description = string.Empty;
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
            owner.Deck.Cards.Push(owner.ActivePokemonCard);
            owner.Deck.Shuffle();

            var message = new SelectFromYourBenchMessage(1).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);
            owner.ForceRetreatActivePokemon((PokemonCard)game.FindCardById(response.Cards.First()));
        }
    }
}
