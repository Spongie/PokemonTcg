using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class KnockBack : Attack
    {
        public KnockBack()
        {
            Name = "Knock Back";
            Description = "If your opponent has any Benched Pokémon, he or she chooses 1 of them and switches it with the Defending Pokémon. (Do the damage before switching the Pokémon.)";
            DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!opponent.BenchedPokemon.Any())
                return;

            var response = opponent.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourBench(1).ToNetworkMessage(opponent.Id));
            opponent.ForceRetreatActivePokemon((PokemonCard)response.Cards.First());
        }
    }
}
