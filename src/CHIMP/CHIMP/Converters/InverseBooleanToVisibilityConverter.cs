using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Chimp.Converters
{
    public sealed class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                var b = (bool)value;
                switch (b)
                {
                    case false:
                        return Visibility.Visible;
                    case true:
                        return Visibility.Collapsed;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
