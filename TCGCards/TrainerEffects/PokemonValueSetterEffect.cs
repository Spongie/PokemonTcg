using System;
using System.Reflection;
using Entities;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    [ValueSetter(typeof(PokemonCard))]
    public class PokemonValueSetterEffect : DataModel, IEffect
    {
        private PropertyInfoData selectedProperty;
        private string valueToSet;

        public string EffectType
        {
            get
            {
                return "Set value on Pokémon";
            }
        }

        public PropertyInfoData SelectedProperty
        {
            get { return selectedProperty; }
            set
            {
                selectedProperty = value;
                FirePropertyChanged();
            }
        }

        public string ValueToSet
        {
            get { return valueToSet; }
            set
            {
                valueToSet = value;
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
            var property = SelectedProperty.ToProperty();
            var type = property.GetType();

            object realValue;

            if (type == typeof(short))
            {
                realValue = short.Parse(ValueToSet);
            }
            else if (type == typeof(int))
            {
                realValue = int.Parse(ValueToSet);
            }
            else if (type == typeof(long))
            {
                realValue = long.Parse(ValueToSet);
            }
            else if (type == typeof(bool))
            {
                realValue = bool.Parse(ValueToSet);
            }
            else
            {
                realValue = ValueToSet;
            }

            property.SetValue(game.LastTarget, realValue);
        }
    }
}
