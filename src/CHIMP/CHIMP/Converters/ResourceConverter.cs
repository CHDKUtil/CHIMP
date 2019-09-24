using Chimp.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Chimp.Converters
{
    public sealed class ResourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string key))
                return null;
            var format = parameter as string ?? "{0}";
            var name = string.Format(format, key);
            var obj = Resources.ResourceManager.GetObject(name);
            return obj ?? name;
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
