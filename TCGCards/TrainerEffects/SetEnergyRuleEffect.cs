using System;
using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class SetEnergyRuleEffect : DataModel, IEffect
    {
        private Type type;
        private bool onlyReplaceIfOldAllows;

        public string EffectType
        {
            get
            {
                return "Set Energy Rule";
            }
        }

        [DynamicInput("Rule", InputControl.Type, typeof(IEnergyRule))]
        public Type RuleType
        {
            get { return type; }
            set
            {
                type = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only replace if old allows", InputControl.Boolean)]
        public bool OnlyReplaceIfOldAllows
        {
            get { return onlyReplaceIfOldAllows; }
            set
            {
                onlyReplaceIfOldAllows = value;
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
            if (RuleType == game.EnergyRule.GetType())
            {
                return;
            }

            if (OnlyReplaceIfOldAllows && !game.EnergyRule.CanPlayEnergyCard(new EnergyCard(), new PokemonCard()))
            {
                return;
            }

            game.EnergyRule = (IEnergyRule)Activator.CreateInstance(RuleType);
        }
    }
}
