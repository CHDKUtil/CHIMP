using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers.Src;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class SrcAddressTreeProvider : SrcProvider<AddressPlatformData, AddressRevisionData, CameraData, RevisionData, ushort>, IInnerAddressTreeProvider
    {
        private IdAddressProvider IdAddressProvider { get; }
        private ILogger Logger { get; }

        public SrcAddressTreeProvider(PlatformProvider platformProvider, IdAddressProvider idAddressProvider, ILogger<SrcAddressTreeProvider> logger)
            : base(platformProvider)
        {
            IdAddressProvider = idAddressProvider;
            Logger = logger;
        }

        public IDictionary<string, AddressPlatformData> GetAddresses(string path)
        {
            var idAddresses = GetIdAddresses(path);

            return GetTree(path)
                .Select(kvp => Update(kvp!, idAddresses!))
                .Where(kvp => kvp.Value.ClearOverlay)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private Dictionary<ushort, uint>? GetIdAddresses(string path)
        {
            var loaderPath = Path.Combine(path, "loader");
            return IdAddressProvider.GetData(loaderPath);
        }

        private KeyValuePair<string, AddressPlatformData> Update(KeyValuePair<string, AddressPlatformData> kvp, Dictionary<ushort, uint> idAddresses)
        {
            if (!idAddresses.TryGetValue(kvp.Value.Id, out var value))
                Logger.LogWarning("{0}: Missing pid_led", kvp.Key);
            else
                kvp.Value.IdAddress = value;
            return kvp;
        }
    }
}
