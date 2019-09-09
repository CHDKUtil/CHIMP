using Net.Chdk.Detectors.Software.Product;
using System;
using System.Globalization;

namespace Net.Chdk.Detectors.Software.Ml
{
    sealed class MlModuleDetector : ProductBinaryModuleDetector
    {
        public override string ProductName => "ML";

        protected override string String => "Build user\0";

        protected override int StringCount => 3;

        protected override string GetChangeset(string[] strings)
        {
            var split = strings[2].Split(' ');
            if (split.Length == 0)
                return null;
            if (split[0].Length != 7)
                return null;
            return split[0];
        }

        protected override DateTime? GetCreationDate(string[] strings)
        {
            var split = strings[0].Split(' ');
            if (split.Length != 3)
                return null;
            var dateStr = string.Format("{0} {1}", split[0], split[1]);
            DateTime date;
            if (!DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out date))
                return null;
            return date;
        }
    }
}
