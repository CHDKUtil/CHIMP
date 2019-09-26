using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Address;
using Net.Chdk.Meta.Providers.Src;
using System;
using System.Collections.Generic;

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
            if (camera?.CleanOverlay != true)
                return null;

            return new AddressPlatformData
            {
                Id = GetId(platformPath, platform, sourcePlatform, revisions)
            };
        }

        protected override ushort? GetValue(KeyValuePair<string, AddressRevisionData> kvp)
        {
            return kvp.Value.Id;
        }

        private ushort GetId(string platformPath, string platform, string? sourcePlatform, IDictionary<string, AddressRevisionData> revisions)
        {
            var data = GetData(platformPath, sourcePlatform, platform);
            return data?.Id
                ?? GetRevisionValue(revisions, platform)
                ?? throw new InvalidOperationException($"{platform}: Missing ID");
        }
    }
}
