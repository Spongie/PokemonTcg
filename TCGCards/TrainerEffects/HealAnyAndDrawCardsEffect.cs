using System;
using System.Collections.Generic;
using System.Text;
using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class HealAnyAndDrawCardsEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode;

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

        public string EffectType
        {
            get
            {
                return "Heal and draw";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return Targeting.GetPossibleTargetsFromMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard).Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);

            var response = caster.NetworkPlayer.SendAndWaitForResponse<AskForAmountMessage>(new AskForAmountMessage() { Info = "How much do you wan't to heal " + target.Name }.ToNetworkMessage(game.Id));

            int wantToHealAmount = response.Answer;

            if (wantToHealAmount < 10)
            {
                wantToHealAmount *= 10;
            }

            int toHeal = Math.Min(target.DamageCounters, wantToHealAmount);

            target.Heal(toHeal, game);
            caster.DrawCards(toHeal / 10);
        }
    }
}
