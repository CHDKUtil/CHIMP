using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Writers.Json;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Writers.Address.Json
{
    sealed class JsonAddressWriter : JsonMetaWriter, IInnerAddressWriter
    {
        public void WriteAddresses(string path, IDictionary<string, AddressPlatformData> addresses)
        {
            WriteJson(path, addresses);
        }
    }
}
