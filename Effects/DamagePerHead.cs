using Entities;
using Entities.Effects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Effects
{
    public class DamagePerHead : Effect
    {
        public DamagePerHead()
        {
            Parameters = new ObservableCollection<Parameter>
            {
                new Parameter {Name = "Amount", Value = "0"}
            };
            Name = "Damage Per Heads";
        }

        public override void Process(Dictionary<EffectValues, object> values)
        {
            int damage = int.Parse((string)Parameters.First().Value);
            int heads = (int)values[EffectValues.HeadsCount];
            values.Add(EffectValues.Damage, new Damage(damage * heads));
        }
    }
}
