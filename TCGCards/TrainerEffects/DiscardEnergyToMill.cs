using CardEditor.Views;
using Entities;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class DiscardEnergyToMill : DataModel, IEffect
    {
        private bool millsOpponent;
        private EnergyTypes energyType;

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

        [DynamicInput("Discards from opponent", InputControl.Boolean)]
        public bool MillsOpponents
        {
            get { return millsOpponent; }
            set
            {
                millsOpponent = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Discard attached energy to Mill";
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
            var choices = caster.ActivePokemonCard.AttachedEnergy.Where(x => x.EnergyType == EnergyType).OfType<Card>().ToList();
            var message = new PickFromListMessage(choices, 0, choices.Count).ToNetworkMessage(game.Id);
            var resoponse = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            foreach (var card in resoponse.Cards.Select(id => game.FindCardById(id)))
            {
                caster.ActivePokemonCard.DiscardEnergyCard((EnergyCard)card, game);
            }

            for (int i = 0; i < resoponse.Cards.Count; i++)
            {
                var card = opponent.Deck.Cards.Pop();
                opponent.DiscardPile.Add(card);
            }
        }
    }
}
