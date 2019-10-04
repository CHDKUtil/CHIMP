using Chimp.Properties;
using Net.Chdk.Model.Software;
using System;
using System.Globalization;

namespace Chimp
{
    static class SoftwareProductInfoExtensions
    {
        public static string GetVersionText(this SoftwareProductInfo product)
        {
            var version = product.Version;
            if (version == null)
                return null;
            string format;
            if (version.MajorRevision < 0)
            {
                format = Resources.ResourceManager.GetString($"Product_Version_{product.Name}_Format_3");
                if (format != null)
                {
                    return string.Format(format, product.VersionPrefix, version.Major, version.Minor, version.Build, product.VersionSuffix);
                }
            }
            else
            {
                format = Resources.ResourceManager.GetString($"Product_Version_{product.Name}_Format");
                if (format != null)
                {
                    return string.Format(format, product.VersionPrefix, version.Major, version.Minor, version.Build, version.MajorRevision, version.MinorRevision);
                }
            }
            format = Resources.ResourceManager.GetString($"Product_Version_{product.Name}_Date_Format");
            if (format != null)
            {
                var date = new DateTime(version.Major, version.Minor, version.Build);
                return string.Format(CultureInfo.InvariantCulture, format, product.VersionPrefix, date);
            }
            return version.ToString();
        }
    }
}
