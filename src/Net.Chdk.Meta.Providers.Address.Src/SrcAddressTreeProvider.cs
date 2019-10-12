using Net.Chdk.Adapters.Platform;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers.Src;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class SrcAddressTreeProvider : SrcProvider<AddressPlatformData, AddressRevisionData, CameraData, RevisionData, ushort>, IInnerAddressTreeProvider
    {
        private const string ProductName = "CHDK";

        private IPlatformAdapter PlatformAdapter { get; }

        public SrcAddressTreeProvider(PlatformProvider platformProvider, IPlatformAdapter platformAdapter)
            : base(platformProvider)
        {
            PlatformAdapter = platformAdapter;
        }

        public IDictionary<string, AddressPlatformData> GetAddresses(string path)
        {
            return GetTree(path)
                .Where(HasAddresses)
                .ToDictionary(Normalize, kvp => kvp.Value!);
        }

        private string Normalize(KeyValuePair<string, AddressPlatformData?> kvp)
        {
            return PlatformAdapter.NormalizePlatform(ProductName, kvp.Key);
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
