using Entities;
using Entities.Effects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Effects
{
    public class DamageEffect : Effect
    {
        public DamageEffect()
        {
            Parameters = new ObservableCollection<Parameter>
            {
                new Parameter {Name = "Amount", Value = 0}
            };
            Name = "Damage";
        }

        public DamageEffect(int damage) :this()
        {
            Parameters.First().Value = damage;
        }

        public override void Process(Dictionary<EffectValues, object> values)
        {
            var damage = (int)Parameters.First().Value;
            values.Add(EffectValues.Damage, new Damage(damage));
        }
    }
}
