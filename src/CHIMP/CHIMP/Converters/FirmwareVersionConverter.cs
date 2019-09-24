using Chimp.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Chimp.Converters
{
    public class FirmwareVersionConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is uint v))
                return null;
            uint major1 = (v >> 24) & 0x0f;
            char major2 = (char)(((v >> 20) & 0x0f) + 0x30);
            char major3 = (char)(((v >> 16) & 0x0f) + 0x30);
            uint rev1 = (v >> 8) & 0x7f;
            char rev2 = (char)(((v >> 4) & 0x0f) + 0x30);
            char rev3 = (char)((v & 0x0f) + 0x30);
            return string.Format(Resources.Camera_FirmwareVersion_Format, major1, major2, major3, rev1, rev2, rev3);
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
