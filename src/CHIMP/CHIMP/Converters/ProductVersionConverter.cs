using Net.Chdk.Model.Software;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Chimp.Converters
{
    public sealed class ProductVersionConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SoftwareProductInfo product))
                return value;
            return product.GetVersionText();
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
