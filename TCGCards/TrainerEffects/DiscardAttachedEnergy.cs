using CardEditor.Views;
using Entities;
using Entities.Models;
using NetworkingCore.Messages;
using System;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class DiscardAttachedEnergy : DataModel, IEffect
    {
        private int amount;
        private TargetingMode targetingMode;
        private EnergyTypes energyType;
        private bool coinFlip;
        private bool useLastCoin;
        private bool checkTails;
        private bool allowUseWithoutTarget;
        private bool askYesNo;

        [DynamicInput("Ask Yes/No", InputControl.Boolean)]
        public bool MayAbility
        {
            get { return askYesNo; }
            set
            {
                askYesNo = value;
                FirePropertyChanged();
            }
        }

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

        [DynamicInput("Use last coin flip?", InputControl.Boolean)]
        public bool UseLastCoin
        {
            get { return useLastCoin; }
            set
            {
                useLastCoin = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Trigger on tails instead?", InputControl.Boolean)]
        public bool CheckTails
        {
            get { return checkTails; }
            set
            {
                checkTails = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Allow use without target", InputControl.Boolean)]
        public bool AllowUseWithoutTarget
        {
            get { return allowUseWithoutTarget; }
            set
            {
                allowUseWithoutTarget = value;
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
            if (AllowUseWithoutTarget)
            {
                return true;
            }

            foreach (var pokemon in Targeting.GetPossibleTargetsFromMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard))
            {
                if (pokemon.AttachedEnergy.Count >= 1)
                {
                    return true;
                }
            }

            game.GameLog.AddMessage($"Cannot cast because no pokemon with atleast 1 energy was found");
            return false;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            throw new NotImplementedException();
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (MayAbility && !AskYesNo(caster, "Discard attached energy?"))
            {
                return;
            }

            var targetValue = CheckTails ? 0 : 1;
            var lastValue = game.LastCoinFlipResult ? 1 : 0;

            if (UseLastCoin && lastValue != targetValue)
            {
                return;
            }
            else if (CoinFlip && game.FlipCoins(1) != targetValue)
            {
                return;
            }

            if (Targeting.GetPossibleTargetsFromMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard).All(x => x.AttachedEnergy.Count == 0))
            {
                return;
            }

            while (true)
            {
                PokemonCard target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

                if (target.AttachedEnergy.Count == 0)
                {
                    continue;
                }

                if (target.AttachedEnergy.Count <= Amount)
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

        private bool AskYesNo(Player caster, string info)
        {
            var message = new YesNoMessage() { Message = info }.ToNetworkMessage(caster.Id);

            return caster.NetworkPlayer.SendAndWaitForResponse<YesNoMessage>(message).AnsweredYes;
        }
    }
}
