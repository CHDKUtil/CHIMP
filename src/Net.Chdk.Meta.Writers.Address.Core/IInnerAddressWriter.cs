using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Address
{
    public interface IInnerAddressWriter : IExtensionProvider
    {
        void WriteAddresses(string path, IDictionary<string, AddressPlatformData> addresses);
    }
}
