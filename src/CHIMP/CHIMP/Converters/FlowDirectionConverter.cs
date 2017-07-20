using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Chimp.Converters
{
    public sealed class FlowDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                culture = value as CultureInfo;
            if (culture == null)
                return null;
            return culture.TextInfo.IsRightToLeft
                ? FlowDirection.RightToLeft
                : FlowDirection.LeftToRight;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
