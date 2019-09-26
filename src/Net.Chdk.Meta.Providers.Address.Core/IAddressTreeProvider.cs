using Net.Chdk.Meta.Model.Address;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Address
{
    public interface IAddressTreeProvider
    {
        IDictionary<string, AddressPlatformData> GetAddresses(string path);
    }
}
