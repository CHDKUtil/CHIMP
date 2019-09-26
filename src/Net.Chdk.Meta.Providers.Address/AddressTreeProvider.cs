using System;
using System.Collections.Generic;
using Net.Chdk.Meta.Model.Address;

namespace Net.Chdk.Meta.Providers.Address
{
    sealed class AddressTreeProvider : SingleExtensionProvider<IInnerAddressTreeProvider>, IAddressTreeProvider
    {
        public AddressTreeProvider(IEnumerable<IInnerAddressTreeProvider> providers)
            : base(providers)
        {
        }

        public IDictionary<string, AddressPlatformData> GetAddresses(string path)
        {
            var provider = GetInnerProvider(path, out string ext);
            if (provider == null)
                throw new InvalidOperationException($"Unknown camera tree extension: {ext}");
            return provider.GetAddresses(path);
        }
    }
}
