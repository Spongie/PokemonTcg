using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Entities.Models
{
    public class DataModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T target, T value, [CallerMemberName] string propertyName = "")
        {
            target = value;
            FirePropertyChanged(propertyName);
        }

        protected void FirePropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
