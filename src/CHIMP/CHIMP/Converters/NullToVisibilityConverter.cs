using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Chimp.Converters
{
    public sealed class NullToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || !Enum.TryParse(parameter.ToString(), out Visibility visibility))
                visibility = Visibility.Collapsed;
            if (value == null)
                return visibility;
            return visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
