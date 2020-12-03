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
            foreach (var pokemon in CardUtil.GetPossibleTargetsFromMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard))
            {
                if (pokemon.AttachedEnergy.Count >= Amount)
                {
                    return true;
                }
            }

            game.GameLog.AddMessage($"Cannot cast because no pokemon with atleast {Amount} energy was found");
            return false;
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
                    for (int i = 0; i < Amount; i++)
                    {
                        target.DiscardEnergyCard(target.AttachedEnergy[0], game);
                    }

                    target.AttachedEnergy.Clear();
                }
                else
                {
                    var message = new PickFromListMessage(target.AttachedEnergy.OfType<Card>().ToList(), Amount);
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
