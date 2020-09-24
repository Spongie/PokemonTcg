using Entities.Effects;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Entities.Models
{
    public class Attack : DataModel
    {
        private ObservableCollection<Effect> effects = new ObservableCollection<Effect>();
        private ObservableCollection<Energy> cost = new ObservableCollection<Energy>();
        private string name;
        private string description;
        private string damageText;

        public ObservableCollection<Energy> Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                FirePropertyChanged();
            }
        }


        public string DamageText
        {
            get { return damageText; }
            set
            {
                damageText = value;
                FirePropertyChanged();
            }
        }
        
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                FirePropertyChanged();
            }
        }


        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged();
            }
        }

        public ObservableCollection<Effect> Effects
        {
            get { return effects; }
            set
            {
                effects = value;
                FirePropertyChanged();
            }
        }


        public void Process()
        {
            var effectValues = new Dictionary<int, object>();
        }
    }
}
