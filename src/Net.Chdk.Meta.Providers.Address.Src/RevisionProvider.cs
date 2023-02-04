using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers.Src;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class RevisionProvider : RevisionProvider<AddressRevisionData, RevisionData>
    {
        private StubsDataProvider StubsDataProvider { get; }
        private IEnumerable<IAddressProvider> AddressProviders { get; }

        public RevisionProvider(SourceProvider sourceProvider, DataProvider dataProvider, StubsDataProvider stubsDataProvider,
            IEnumerable<IAddressProvider> addressProviders, ILogger<RevisionProvider> logger)
            : base(sourceProvider, dataProvider, logger)
        {
            StubsDataProvider = stubsDataProvider;
            AddressProviders = addressProviders;
        }

        protected override AddressRevisionData GetRevisionData(string platformPath, string platform, string revision, RevisionData? data)
        {
            var data2 = StubsDataProvider.GetData(platformPath, platform, revision);
            var id = data?.Id ?? data2.Id;
            if (id != null)
                Logger.LogTrace("ID: {0}", id);
            var idAddress = data?.IdAddress ?? data2.IdAddress;
            if (idAddress != null)
                Logger.LogTrace("ID Address: {0}", idAddress);
            var revisionAddress = GetRevisionAddress(data2, platform, revision);
            var addresses = GetAddresses(platformPath, platform, revision);
            var paletteBufferPtr = addresses?.PaletteBufferPtr;
            if (paletteBufferPtr != null)
                Logger.LogTrace("Palette Buffer Pointer: {0}", paletteBufferPtr);
            uint? activePaletteBuffer = addresses?.ActivePaletteBuffer;
            if (activePaletteBuffer != null)
                Logger.LogTrace("Active Palette Buffer: {0}", activePaletteBuffer);

            return new AddressRevisionData
            {
                Id = id,
                IdAddress = idAddress,
                RevisionAddress = revisionAddress,
                PaletteBufferPtr = paletteBufferPtr,
                ActivePaletteBuffer = activePaletteBuffer
            };
        }

        private uint GetRevisionAddress(StubsData data, string platform, string revision)
        {
            var address = data.RevisionAddress;
            if (address == null)
                throw new InvalidOperationException($"{platform}-{revision}: Missing revision address");
            return address.Value;
        }

        private AddressData? GetAddresses(string platformPath, string platform, string revision)
        {
            return AddressProviders
                .Select(p => p.GetAddresses(platformPath, platform, revision))
                .FirstOrDefault(a => a != null);
        }
    }
}
