using Entities.Models;

namespace Entities.Effects
{
    public class Parameter : DataModel
    {
        private string name;
        private string value;

        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
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

    }
}
