using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Entities.Effects
{
    public interface IEffect
    {
        void Process(Dictionary<int, object> values);
        string Name { get; }
        ObservableCollection<Parameter> Parameters { get; }
    }
}
