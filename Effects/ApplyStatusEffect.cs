using Entities;
using Entities.Effects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Effects
{
    public class ApplyStatusEffect : Effect
    {
        public ApplyStatusEffect()
        {
            Parameters = new ObservableCollection<Parameter>
            {
                new Parameter 
                { 
                    Name = "Status", Value = StatusEffect.Sleep,
                    ShowTextInput = false,
                    ShowStatusInput = true
                }
            };
            Name = "Apply Status Effect";
        }

        public override void Process(Dictionary<EffectValues, object> values)
        {
            throw new NotImplementedException();
        }
    }
}
