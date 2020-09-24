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
                new Parameter {Name = "Amount", Value = "0"}
            };
            Name = "Damage";
        }

        public override void Process(Dictionary<EffectValues, object> values)
        {
            int damage = int.Parse((string)Parameters.First().Value);
            values.Add(EffectValues.Damage, new Damage(damage));
        }
    }
}
