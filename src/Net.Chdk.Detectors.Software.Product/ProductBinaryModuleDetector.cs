using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Detectors.Software.Product
{
    public abstract class ProductBinaryModuleDetector : ProductBinaryDetector, IProductBinaryModuleDetector
    {
        public ModuleInfo GetModule(SoftwareInfo software, byte[] buffer, int index, string hashName)
        {
            var strings = GetStrings(buffer, index, StringCount, SeparatorChar);
            if (strings == null)
                return null;

            return new ModuleInfo
            {
                Created = GetCreationDate(strings),
                Changeset = GetChangeset(strings),
                Hash = GetHash(hashName),
            };
        }

        private static SoftwareHashInfo GetHash(string hashName)
        {
            return new SoftwareHashInfo
            {
                Name = hashName,
                Values = new Dictionary<string, string>(),
            };
        }

        protected abstract string GetChangeset(string[] strings);

        protected abstract DateTime? GetCreationDate(string[] strings);
    }
}
