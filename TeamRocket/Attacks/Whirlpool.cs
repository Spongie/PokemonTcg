using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class Whirlpool : Attack
    {
        public Whirlpool()
        {
            Name = "Whirlpool";
            Description = "If the Defending Pokémon has any Energy cards attached to it, choose 1 of them and discard it.";
            DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!opponent.ActivePokemonCard.AttachedEnergy.Any())
            {
                return;
            }
            else if (opponent.ActivePokemonCard.AttachedEnergy.Count == 1)
            {
                var energyCard = opponent.ActivePokemonCard.AttachedEnergy[0];
                opponent.ActivePokemonCard.DiscardEnergyCard(energyCard);
                return;
            }

            var message = new PickFromListMessage(opponent.ActivePokemonCard.AttachedEnergy, 1).ToNetworkMessage(owner.Id);
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);
            opponent.ActivePokemonCard.DiscardEnergyCard((EnergyCard)game.FindCardById(response.Cards.First()));
        }
    }
}
