using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class CoinHurl : Attack
    {
        public CoinHurl()
        {
            Name = "Coin Hurl";
            Description = "Choose 1 of your opponent's Pokémon and flip a coin. If heads, this attack does 20 damage to that Pokémon. Don't apply Weakness and Resistance for this attack. (Any other effects that would happen after applying Weakness and Resistance still happen.)";
            DamageText = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromOpponentBench(1).ToNetworkMessage(owner.Id));

            var selectedPokemon = response.Cards.OfType<PokemonCard>().FirstOrDefault();

            if (selectedPokemon != null)
            {
                selectedPokemon.DamageCounters += 20;
            }
        }
    }
}
