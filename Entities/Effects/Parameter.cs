using Entities.Models;

namespace Entities.Effects
{
    public class Parameter : DataModel
    {
        private string name;
        private object value;

        public object Value
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

        public bool ShowTextInput { get; set; } = true;
        public bool ShowStatusInput { get; set; } = false;
        public bool ShowTypeInput { get; set; } = false;
    }
}
