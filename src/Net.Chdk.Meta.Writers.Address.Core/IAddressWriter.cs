using Net.Chdk.Meta.Model.Address;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Address
{
    public interface IAddressWriter
    {
        void WriteAddresses(string path, IDictionary<string, AddressPlatformData> addresses);
    }
}
