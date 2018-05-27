using System.Collections.Generic;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.PokemonCards.TeamRocket.Attacks
{
    public class Vanish : Attack
    {
        public Vanish()
        {
            Name = "Vanish";
            Description = string.Empty;
            NeedsPlayerInteraction = true;
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
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
            owner.SetActivePokemon(response.ActivePokemon);
        }
    }
}
