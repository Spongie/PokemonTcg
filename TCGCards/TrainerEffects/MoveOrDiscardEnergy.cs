using System.Linq;
using CardEditor.Views;
using Entities;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class MoveOrDiscardEnergy : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Move or discard energy";
            }
        }

        private EnergyTypes energyType = EnergyTypes.All;
        private int amount;
        private TargetingMode sourceTargetingMode = TargetingMode.Self;
        private TargetingMode newTargetingMode = TargetingMode.YourPokemon;

        [DynamicInput("Energytype", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Amount")]
        public int AmountToMove
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Source Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode SourceTargetingMode
        {
            get { return sourceTargetingMode; }
            set
            {
                sourceTargetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("New Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode NewTargetingMode
        {
            get { return newTargetingMode; }
            set
            {
                newTargetingMode = value;
                FirePropertyChanged();
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var source = Targeting.AskForTargetFromTargetingMode(SourceTargetingMode, game, caster, opponent, pokemonSource, "Select Pokémon to Move energy from");

            var availableEnergy = source.AttachedEnergy.Where(e => EnergyType == EnergyTypes.All || e.EnergyType == EnergyType).ToList();

            if (availableEnergy.Count > AmountToMove)
            {
                var pickEnergyMessage = new PickFromListMessage(availableEnergy.OfType<Card>().ToList(), AmountToMove).ToNetworkMessage(game.Id);
                var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(pickEnergyMessage);

                availableEnergy.Clear();
                foreach (var id in response.Cards)
                {
                    availableEnergy.Add((EnergyCard)game.Cards[id]);
                }
            }

            var newTarget = Targeting.AskForTargetFromTargetingMode(NewTargetingMode, game, caster, opponent, pokemonSource, "Select Pokémon to Move energy from");

            if (newTarget == null)
            {
                foreach (var energy in availableEnergy)
                {
                    source.DiscardEnergyCard(energy, game);
                }
            }
            else
            {
                foreach (var energy in availableEnergy)
                {
                    source.AttachedEnergy.Remove(energy);
                    game.SendEventToPlayers(new AttachedEnergyDiscardedEvent() { DiscardedCard = energy, FromPokemonId = source.Id });
                    newTarget.AttachEnergy(energy, game);
                }
            }
        }
    }
}
