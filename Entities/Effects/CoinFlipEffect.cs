using Entities.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Entities.Effects
{
    public class CoinFlipEffect : DataModel, IEffect
    {
        private ObservableCollection<Parameter> parameters;

        public CoinFlipEffect()
        {
            Parameters = new ObservableCollection<Parameter>
            {
                new Parameter{Name = "Amount", Value = "1"}
            };
        }

        public string Name
        {
            get => "Coin Flip";
        }

        public ObservableCollection<Parameter> Parameters
        {
            get { return parameters; }
            set
            {
                parameters = value;
                FirePropertyChanged();
            }
        }


        public void Process(Dictionary<int, object> values)
        {
            var heads = 1;
            var tails = 2;

            values.Add((int)EffectValues.HeadsCount, heads);
            values.Add((int)EffectValues.HeadsCount, tails);
        }
    }
}
