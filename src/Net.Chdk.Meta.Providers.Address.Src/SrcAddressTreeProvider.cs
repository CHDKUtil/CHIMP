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
                .Where(HasAddresses)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!);
        }

        private static bool HasAddresses(KeyValuePair<string, AddressPlatformData?> kvp)
        {
            return kvp.Value?.Thumb == true && kvp.Value!.Revisions.Any(HasAddresses);
        }

        private static bool HasAddresses(KeyValuePair<string, AddressRevisionData> kvp)
        {
            return kvp.Value.PaletteBufferPtr != null;
        }
    }
}
