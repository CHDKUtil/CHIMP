using System;
using System.Globalization;
using System.Windows.Data;

namespace Chimp.Converters
{
    sealed class DebugConverter : IValueConverter, IMultiValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object? Convert(object[]? values, Type targetType, object parameter, CultureInfo culture)
        {
            return values;
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object[]? ConvertBack(object? value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return value != null ? new[] { value } : new object[0];
        }
    }
}
