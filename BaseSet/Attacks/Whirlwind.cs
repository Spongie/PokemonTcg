using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace BaseSet.Attacks
{
    internal class Whirlwind : Attack
    {
        public Whirlwind()
        {
            Name = "Whirlwind";
            Description = "If your opponent has any Benched Pokémon, he or she chooses 1 of them and switches it with the Defending Pokémon. (Do the damage before switching the Pokémon.)";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!opponent.BenchedPokemon.Any())
                return;

            var response = opponent.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourBench(1).ToNetworkMessage(opponent.Id));
            var newActive = (PokemonCard)game.FindCardById(response.Cards.First());
            opponent.ForceRetreatActivePokemon(newActive);
        }
    }
}
