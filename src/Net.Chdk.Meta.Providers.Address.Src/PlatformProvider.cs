using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers.Src;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class PlatformProvider : PlatformProvider<AddressPlatformData, AddressRevisionData, CameraData, RevisionData, ushort>
    {
        public PlatformProvider(RevisionProvider revisionProvider, CameraProvider cameraProvider, DataProvider dataProvider, ILogger<PlatformProvider> logger)
            : base(revisionProvider, cameraProvider, dataProvider, logger)
        {
        }

        protected override AddressPlatformData? GetPlatformData(string platformPath, string platform, string? sourcePlatform, IDictionary<string, AddressRevisionData> revisions, CameraData? camera)
        {
            var data = GetData(platformPath, sourcePlatform, platform);
            return new AddressPlatformData
            {
                Id = GetId(data, revisions, platform),
                IdAddress = GetIdAddress(data, revisions, platform),
                ClearOverlay = camera?.CleanOverlay == true
            };
        }

        protected override ushort? GetValue(KeyValuePair<string, AddressRevisionData> kvp)
        {
            return kvp.Value.Id;
        }

        private ushort GetId(RevisionData? data, IDictionary<string, AddressRevisionData> revisions, string platform)
        {
            return data?.Id
                ?? GetRevisionValue(revisions, platform)
                ?? throw new InvalidOperationException($"{platform}: Missing ID");
        }

        private uint GetIdAddress(RevisionData? data, IDictionary<string, AddressRevisionData> revisions, string platform)
        {
            return data?.IdAddress
                ?? GetRevisionIdAddress(revisions, platform)
                ?? throw new InvalidOperationException($"{platform}: Missing ID address");
        }

        private uint? GetRevisionIdAddress(IDictionary<string, AddressRevisionData> revisions, string platform)
        {
            var value = revisions
                .Select(kvp => kvp.Value.IdAddress)
                .FirstOrDefault(v => v != null);
            if (revisions.Any(kvp => kvp.Value.IdAddress?.Equals(value) == false))
                throw new InvalidOperationException($"{platform}: Mismatching ID address");
            return value;
        }
    }
}
