using Microsoft.Extensions.Logging;
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
        private ILogger Logger { get; }

        public SrcAddressTreeProvider(PlatformProvider platformProvider, IPlatformAdapter platformAdapter, ILogger<SrcAddressTreeProvider> logger)
            : base(platformProvider)
        {
            PlatformAdapter = platformAdapter;
            Logger = logger;
        }

        public IDictionary<string, AddressPlatformData> GetAddresses(string path)
        {
            return GetTree(path)
                .Where(kvp => kvp.Value!.ClearOverlay)
                .ToDictionary(Normalize, kvp => kvp.Value!);
        }

        private string Normalize(KeyValuePair<string, AddressPlatformData?> kvp)
        {
            return PlatformAdapter.NormalizePlatform(ProductName, kvp.Key);
        }
    }
}
