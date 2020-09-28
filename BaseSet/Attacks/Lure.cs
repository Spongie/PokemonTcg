using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.Attacks
{
    internal class Lure : Attack
    {
        public Lure()
        {
            Name = "Lure";
            Description = "If your opponent has any Benched Pokémon, choose 1 of them and switch it with the Defending Pokémon.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!opponent.BenchedPokemon.Any())
                return;

            var response = opponent.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourBenchMessage(1).ToNetworkMessage(opponent.Id));
            var newActive = (PokemonCard)game.FindCardById(response.Cards.First());
            opponent.ForceRetreatActivePokemon(newActive);
        }
    }
}
