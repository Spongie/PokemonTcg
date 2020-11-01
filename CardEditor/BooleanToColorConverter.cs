using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CardEditor
{
    public class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Brushes.Black;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Brushes)value;

            return color.Equals(Brushes.White);
        }
    }
}
