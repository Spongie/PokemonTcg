using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class SelectStatusToApplyEffect : DataModel, IEffect
    {
        private bool canSelectParalyze;
        private bool canSelectConfuse;
        private bool canSelectBurn;
        private bool canSelectPoison;
        private bool canSelectSleep;

        private TargetingMode targetingMode;
        private bool coinFlip;

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Can Paralyze", InputControl.Boolean)]
        public bool CanSelectParalyze
        {
            get { return canSelectParalyze; }
            set
            {
                canSelectParalyze = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Can Sleep", InputControl.Boolean)]
        public bool CanSelectSleep
        {
            get { return canSelectSleep; }
            set
            {
                canSelectSleep = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Can Poison", InputControl.Boolean)]
        public bool CanSelectPoison
        {
            get { return canSelectPoison; }
            set
            {
                canSelectPoison = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Can Burn", InputControl.Boolean)]
        public bool CanSelectBurn
        {
            get { return canSelectBurn; }
            set
            {
                canSelectBurn = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Can Confuse", InputControl.Boolean)]
        public bool CanSelectConfuse
        {
            get { return canSelectConfuse; }
            set
            {
                canSelectConfuse = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("CoinFlip", InputControl.Boolean)]
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
                return "Apply selected status";
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
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            var response = caster.NetworkPlayer.SendAndWaitForResponse<StatusEffectMessage>(new PickStatusMessage()
            {
                CanBurn = CanSelectBurn,
                CanConfuse = CanSelectConfuse,
                CanParalyze = CanSelectParalyze,
                CanPoison = CanSelectPoison,
                CanSleep = CanSelectSleep
            }.ToNetworkMessage(game.Id));

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource, "Select Pokémon to apply effect to");

            target.ApplyStatusEffect(response.StatusEffect, game);
        }
    }
}
