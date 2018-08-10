using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class DigUnder : Attack
    {
        public DigUnder()
        {
            Name = "Dig Under";
            Description = "Choose 1 of your opponent's Pokémon. This attack does 10 damage to that Pokémon. Don't apply Weakness and Resistance for this attack. (Any other effects that would happen after applying Weakness and Resistance still happen.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var response = owner.NetworkPlayer.SendAndWaitForResponse<PokemonCardListMessage>(new SelectOpponentPokemon(1).ToNetworkMessage(owner.Id));

            if (response.Pokemons.Any())
            {
                response.Pokemons.First().DamageCounters += 10;
            }
        }
    }
}
