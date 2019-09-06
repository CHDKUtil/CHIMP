using System.Globalization;

namespace Net.Chdk
{
    public static class CultureInfoExtensions
    {
        public static bool IsCurrentUICulture(this CultureInfo culture)
        {
            for (var currentCulture = CultureInfo.CurrentUICulture; currentCulture != null; currentCulture = currentCulture.Parent)
            {
                if (currentCulture.Equals(culture))
                    return true;
                if (currentCulture.IsNeutralCulture)
                    return false;
            }
            return false;
        }
    }
}
