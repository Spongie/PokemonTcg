using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class Fascinate : Attack
    {
        public Fascinate()
        {
            Name = "Fascinate";
            Description = "Flip a coin. If heads, choose 1 of your opponent's Benched Pokémon and switch it with the Defending Pokémon. This attack can't be used if your opponent has no Benched Pokémon.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!CoinFlipper.FlipCoin())
                return;

            var message = new SelectFromOpponentBench(1).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<PokemonCardListMessage>(message);

            opponent.ForceRetreatActivePokemon(response.Pokemons.First());   
        }

        public override bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            return base.CanBeUsed(game, owner, opponent) && opponent.BenchedPokemon.Any();
        }
    }
}
