using Net.Chdk.Meta.Model.Address;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Address
{
    public interface IInnerAddressTreeProvider : IExtensionProvider
    {
        IDictionary<string, AddressPlatformData> GetAddresses(string path);
    }
}
