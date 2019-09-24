using Chimp.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Chimp.Converters
{
    public sealed class SizeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            ulong b = System.Convert.ToUInt64(value);

            if (b < 1024)
                return string.Format(Resources.DiskSpace_Bytes_Format, b);

            double kb = b / 1024;

            if (kb < 1024)
                return string.Format(Resources.DiskSpace_KBytes_Format, kb);

            double mb = kb / 1024;

            if (mb < 1024)
                return string.Format(Resources.DiskSpace_MBytes_Format, mb);

            double gb = mb / 1024;

            return string.Format(Resources.DiskSpace_GBytes_Format, gb);
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
