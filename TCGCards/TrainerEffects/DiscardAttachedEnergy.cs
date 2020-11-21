using CardEditor.Views;
using Entities;
using Entities.Models;
using System;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class DiscardAttachedEnergy : DataModel, IEffect
    {
        private int amount;
        private TargetingMode targetingMode;
        private EnergyTypes energyType;
        private bool coinFlip;

        [DynamicInput("Targeting type", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Cards to discard")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Energy type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Flips Coin?", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Discard attached energy";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            throw new NotImplementedException();
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            while (true)
            {
                PokemonCard target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

                if (target.AttachedEnergy.Count < Amount)
                {
                    continue;
                }

                if (target.AttachedEnergy.Count == Amount)
                {
                    target.Owner.DiscardPile.AddRange(target.AttachedEnergy);
                    target.AttachedEnergy.Clear();
                }
                else
                {
                    var message = new PickFromListMessage(target.AttachedEnergy, Amount);
                    var ids = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(game.Id)).Cards;

                    var cards = target.AttachedEnergy.Where(x => ids.Contains(x.Id)).ToList();

                    foreach (var card in cards)
                    {
                        target.DiscardEnergyCard(card, game);
                    }
                }

                break;
            }
        }
    }
}
