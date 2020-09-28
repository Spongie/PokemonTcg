using NetworkingCore.Messages;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.EnergyCards;

namespace TeamRocket.Attacks
{
    internal class FlamePillar : Attack
    {
        public FlamePillar()
        {
            Name = "Flame Pillar";
            Description = "You may discard 1 [R] Energy card attached to Dark Rapidash when you use this attack. If you do and your opponent has any Benched Pokémon, choose 1 of them and this attack does 10 damage to it. (Don't apply Weakness and Resistance for Benched Pokémon.)";
            DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            if (opponent.BenchedPokemon.Any() && owner.ActivePokemonCard.AttachedEnergy.OfType<FireEnergy>().Any())
            {
                var activateMessage = new YesNoMessage { Message = Description }.ToNetworkMessage(owner.Id);
                var activateResponse = owner.NetworkPlayer.SendAndWaitForResponse<YesNoMessage>(activateMessage);

                if (activateResponse.AnsweredYes)
                {
                    var pickEnergyMessage = new PickFromListMessage(owner.ActivePokemonCard.AttachedEnergy.OfType<FireEnergy>(), 1);
                    var energyResponse = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(pickEnergyMessage.ToNetworkMessage(owner.Id));

                    var selectedEnergy = owner.ActivePokemonCard.AttachedEnergy.First(x => x.Id.Equals(energyResponse.Cards.First()));
                    owner.ActivePokemonCard.DiscardEnergyCard(selectedEnergy);

                    var message = new SelectFromOpponentBenchMessage(1).ToNetworkMessage(owner.Id);
                    var selectedId = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    var pokemon = opponent.BenchedPokemon.First(x => x.Id.Equals(selectedId));
                    pokemon.DamageCounters += 10;
                }
            }

            return 30;
        }
    }
}
