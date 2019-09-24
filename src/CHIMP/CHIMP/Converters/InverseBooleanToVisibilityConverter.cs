using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Chimp.Converters
{
    public sealed class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool b))
                return null;
            return b switch
            {
                false => Visibility.Visible,
                true => Visibility.Collapsed
            };
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
