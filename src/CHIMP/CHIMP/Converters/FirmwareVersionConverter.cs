using System;
using System.Globalization;
using System.Windows.Data;

namespace Chimp.Converters
{
    public class FirmwareVersionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is uint v))
                return null;
            uint major1 = (v >> 24) & 0x0f;
            char major2 = (char)(((v >> 20) & 0x0f) + 0x30);
            char major3 = (char)(((v >> 16) & 0x0f) + 0x30);
            char rev = (char)(((v >> 8) & 0x1f) + 0x40);
            return $"{major1}.{major2}{major3}{rev}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
