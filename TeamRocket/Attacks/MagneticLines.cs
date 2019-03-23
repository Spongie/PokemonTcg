using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.Messages;

namespace TeamRocket.Attacks
{
    internal class MagneticLines : Attack
    {
        public MagneticLines()
        {
            Name = "Magnetic Lines";
            Description = "If the Defending Pokémon has any basic Energy cards attached to it, choose 1 of them. If your opponent have any Benched Pokémon, choose 1 of them and attach that Energy card to it.";
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
            if (!opponent.ActivePokemonCard.AttachedEnergy.Any(x => x.IsBasic) || !opponent.BenchedPokemon.Any())
                return;

            var selectedEnergyMessage = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new PickFromListMessage(opponent.ActivePokemonCard.AttachedEnergy.OfType<EnergyCard>().Where(card => card.IsBasic), 1).ToNetworkMessage(owner.Id));
            var selectedPokemonMessage = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromOpponentBench(1).ToNetworkMessage(owner.Id));
            var selectedEnergy = selectedEnergyMessage.Cards.OfType<EnergyCard>().First();

            opponent.AttachEnergyToPokemon(selectedEnergy, (PokemonCard)selectedPokemonMessage.Cards.First());
            opponent.ActivePokemonCard.AttachedEnergy.Remove(selectedEnergy);
        }
    }
}
