using System;
using System.Globalization;
using System.Windows.Data;

namespace Chimp.Converters
{
    public sealed class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return InverseBoolean(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return InverseBoolean(value);
        }

        private static object InverseBoolean(object value)
        {
            var b = value as bool?;
            switch (b)
            {
                case false:
                    return true;
                case true:
                    return false;
                default:
                    return null;
            }
        }
    }
}
