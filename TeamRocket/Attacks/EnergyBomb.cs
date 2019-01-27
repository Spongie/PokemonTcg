using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class EnergyBomb : Attack
    {
        public EnergyBomb()
        {
            Name = "Energy Bomb";
            Description = "Take all Energy cards attached to Dark Electrode and attach them to your Benched Pokémon (in any way you choose). If you have no Benched Pokémon, discard all Energy cards attached to Dark Electrode.";
            DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            var energyCards = owner.ActivePokemonCard.AttachedEnergy;
            var message = new AttachEnergyCardsToBenchMessage(energyCards).ToNetworkMessage(owner.Id);
            owner.ActivePokemonCard.AttachedEnergy.Clear();
            var response = owner.NetworkPlayer.SendAndWaitForResponse<AttachedEnergyDoneMessage>(message);

            foreach (var energyId in response.EnergyPokemonMap.Keys)
            {
                var attachesToId = response.EnergyPokemonMap[energyId];
                owner.AttachEnergyToPokemon(energyCards.First(energy => energy.Id.Equals(energyId)), owner.BenchedPokemon.First(pokemon => pokemon.Id.Equals(attachesToId)));
            }
        }
    }
}
