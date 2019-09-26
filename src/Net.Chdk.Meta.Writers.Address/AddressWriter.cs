using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Address
{
    sealed class AddressWriter : SingleExtensionProvider<IInnerAddressWriter>, IAddressWriter
    {
        public AddressWriter(IEnumerable<IInnerAddressWriter> writers)
            : base(writers)
        {
        }

        public void WriteAddresses(string path, IDictionary<string, AddressPlatformData> addresses)
        {
            var writer = GetInnerProvider(path, out string ext);
            if (writer == null)
                throw new InvalidOperationException($"Unknown address writer extension: {ext}");
            writer.WriteAddresses(path, addresses);
        }
    }
}
