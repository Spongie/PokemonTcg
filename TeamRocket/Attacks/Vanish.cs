using System.Collections.Generic;
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

            game.GameState = GameFieldState.ActivePlayerSelectingFromBench;

            var message = new GameFieldMessage(game).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<ActiveSelectedMessage>(message);
            owner.ForceRetreatActivePokemon(response.SelectedPokemon);
        }
    }
}
