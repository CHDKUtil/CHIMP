using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers.Src;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class SrcAddressTreeProvider : SrcProvider<AddressPlatformData, AddressRevisionData, CameraData, RevisionData, ushort>, IInnerAddressTreeProvider
    {
        private ILogger Logger { get; }

        public SrcAddressTreeProvider(PlatformProvider platformProvider, ILogger<SrcAddressTreeProvider> logger)
            : base(platformProvider)
        {
            Logger = logger;
        }

        public IDictionary<string, AddressPlatformData> GetAddresses(string path)
        {
            return GetTree(path)
                .Where(kvp => kvp.Value!.ClearOverlay)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!);
        }
    }
}
