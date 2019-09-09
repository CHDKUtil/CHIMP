using System.Collections.Generic;
using System.Globalization;

namespace Net.Chdk.Detectors.Software.Product
{
    public abstract class ProductDetectorBase
    {
        private static readonly Dictionary<string, CultureInfo> CultureInfos = new Dictionary<string, CultureInfo>();

        protected static CultureInfo GetCultureInfo(string name)
        {
            if (!(CultureInfos.TryGetValue(name, out CultureInfo value)))
            {
                value = new CultureInfo(name);
                CultureInfos.Add(name, value);
            }
            return value;
        }
    }
}
