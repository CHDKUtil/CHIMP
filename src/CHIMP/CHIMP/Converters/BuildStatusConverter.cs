using Chimp.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Chimp.Converters
{
    public sealed class BuildStatusConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string str))
                return null;
            var key = str.Length > 0 ? str.ToLowerInvariant() : "final";
            return Resources.ResourceManager.GetString($"Software_Status_{key}") ?? str;
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
