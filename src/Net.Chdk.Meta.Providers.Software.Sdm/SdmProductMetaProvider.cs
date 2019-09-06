using Net.Chdk.Model.Software;
using System;
using System.Globalization;
using System.IO;

namespace Net.Chdk.Meta.Providers.Software.Sdm
{
    sealed class SdmProductMetaProvider : IProductMetaProvider
    {
        private static readonly CultureInfo Language = new CultureInfo("en");

        public SoftwareProductInfo GetProduct(string name, DateTime created)
        {
            var split = Path.GetFileNameWithoutExtension(name).Split('-');
            return new SoftwareProductInfo
            {
                Name = split[0],
                Version = Version.Parse(split[4]),
                Language = Language,
                Created = created
            };
        }
    }
}
