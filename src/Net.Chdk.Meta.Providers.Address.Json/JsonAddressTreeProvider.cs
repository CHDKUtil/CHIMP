using System.Collections.Generic;
using Net.Chdk.Meta.Model;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers.Json;

namespace Net.Chdk.Meta.Providers.Address.Json
{
    sealed class JsonAddressTreeProvider : JsonCameraProvider<AddressPlatformData, AddressRevisionData, PlatformSourceData>, IInnerAddressTreeProvider
    {
        public IDictionary<string, AddressPlatformData> GetAddresses(string path)
        {
            return GetCameras(path);
        }
    }
}
