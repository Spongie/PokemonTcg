using Entities.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Entities.Effects
{
    public abstract class Effect : DataModel
    {
        private ObservableCollection<Parameter> parameters;

        public abstract void Process(Dictionary<EffectValues, object> values);
        public string Name { get; protected set; }

        public ObservableCollection<Parameter> Parameters
        {
            get { return parameters; }
            set
            {
                parameters = value;
                FirePropertyChanged();
            }
        }
    }
}
