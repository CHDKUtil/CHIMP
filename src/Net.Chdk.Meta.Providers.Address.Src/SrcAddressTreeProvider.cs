using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers.Src;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class SrcAddressTreeProvider : SrcProvider<AddressPlatformData, AddressRevisionData, CameraData, RevisionData, ushort>, IInnerAddressTreeProvider
    {
        public SrcAddressTreeProvider(PlatformProvider platformProvider)
            : base(platformProvider)
        {
        }

        public IDictionary<string, AddressPlatformData> GetAddresses(string path)
        {
            return GetTree(path)
                .Where(kvp => kvp.Value != null)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)!;
        }
    }
}
