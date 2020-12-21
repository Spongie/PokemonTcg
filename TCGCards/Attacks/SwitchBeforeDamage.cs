using Entities;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.Attacks
{
    public class SwitchBeforeDamage : Attack
    {
        public override bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            return opponent.BenchedPokemon.Count > 0 && base.CanBeUsed(game, owner, opponent);
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            opponent.NetworkPlayer.Send(new InfoMessage("Opponent is selecting a new active Pokémon for you").ToNetworkMessage(Id));
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromOpponentBenchMessage().ToNetworkMessage(game.Id));

            var newActivePokemon = (PokemonCard)game.Cards[response.Cards.First()];
            opponent.ForceRetreatActivePokemon(newActivePokemon, game);

            return base.GetDamage(owner, opponent, game);
        }
    }
}
