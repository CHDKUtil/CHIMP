using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Crypto;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Software
{
    sealed class SoftwareHashProvider : ISoftwareHashProvider
    {
        private IHashProvider HashProvider { get; }

        public SoftwareHashProvider(IHashProvider hashProvider)
        {
            HashProvider = hashProvider;
        }

        public SoftwareHashInfo GetHash(byte[] buffer, string fileName, string hashName)
        {
            var value = HashProvider.GetHashString(buffer, hashName);
            return new SoftwareHashInfo
            {
                Name = hashName,
                Values = GetHashValues(fileName, value)
            };
        }

        private Dictionary<string, string> GetHashValues(string fileName, string value)
        {
            var key = fileName.ToLowerInvariant();
            return new Dictionary<string, string>
            {
                { key, value }
            };
        }
    }
}
