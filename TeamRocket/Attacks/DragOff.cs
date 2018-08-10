using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class DragOff : Attack
    {
        public DragOff()
        {
            Name = "Drag Off";
            Description = "Before doing damage, choose 1 of your opponent's Benched Pokémon and switch it with the Defending Pokémon. Do the damage to the new Defending Pokémon. This attack can't be used if your opponent has not Benched Pokémon.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            var response = owner.NetworkPlayer.SendAndWaitForResponse<PokemonCardListMessage>(new SelectFromOpponentBench(1).ToNetworkMessage(owner.Id));

            IPokemonCard currentActive = opponent.ActivePokemonCard;
            IPokemonCard newActive = response.Pokemons.First();

            opponent.ActivePokemonCard = newActive;
            opponent.BenchedPokemon.Remove(newActive);
            opponent.BenchedPokemon.Add(currentActive);

            return 20;
        }

        public override bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            return base.CanBeUsed(game, owner, opponent) && opponent.BenchedPokemon.Any();
        }
    }
}
