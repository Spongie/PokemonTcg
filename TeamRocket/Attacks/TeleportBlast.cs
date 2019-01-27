using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class TeleportBlast : Attack
    {
        public TeleportBlast()
        {
            Name = "Teleport Blast";
            Description = "You may switch Dark Alakazam with 1 of your Benched Pokémon. (Do the damage before switching the Pokémon.)";
            DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var response = owner.NetworkPlayer.SendAndWaitForResponse<PokemonCardListMessage>(new SelectFromYourBench(1).ToNetworkMessage(owner.Id));
            owner.ForceRetreatActivePokemon(response.Pokemons.First());
        }
    }
}
